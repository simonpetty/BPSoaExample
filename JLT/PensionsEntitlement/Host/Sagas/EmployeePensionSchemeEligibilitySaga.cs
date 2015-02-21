using System;
using System.Collections.Generic;
using JLT.OnlineBenefits.Messages.RequestResponse;
using MassTransit;
using MassTransit.Saga;
using Magnum.StateMachine;
using System.Linq;
using JLT.PensionsEntitlement.Messages.Events;
using JLT.OnlineBenefits.Messages.Events;

namespace JLT.PensionsEntitlement.Host.Sagas
{
    public class PensionSchemeEligibilityProfile
    {
        public Guid Id { get; set; }

        public string EligibilityFormula { get; set; }

        public bool? IsEligible { get; set; }

        public List<Guid> PensionSchemeIds { get; set; }
    }

    public class EmployeePensionSchemeEligibilitySaga : SagaStateMachine<EmployeePensionSchemeEligibilitySaga>, ISaga
    {
        public IServiceBus Bus { get; set; }

        public Guid CorrelationId { get; set; }

        public List<PensionSchemeEligibilityProfile> Profiles { get; set; }

        public EmployeePensionSchemeEligibilitySaga(Guid correlationId)
        {
            CorrelationId = correlationId;
        }

        public static State Initial { get; set; }
        public static State OnGoing { get; set; }
        public static State Completed { get; set; }

        public static Event<EmployeeCreated> EmployeeCreated { get; set; }
        public static Event<IFormulaTokenValueChanged> FormulaTokenValueChanged { get; set; }
        public static Event<IProfileCreated> ProfileCreated { get; set; }
        public static Event<EvaluatePensionEligibilityResponse> ProfileEligibilityEvaluated { get; set; }

        static EmployeePensionSchemeEligibilitySaga()
        {
            Define(() =>
            {
                Correlate(EmployeeCreated)
                    .By((saga, message) => saga.CorrelationId == message.EmployeeId)
                    .UseId(x => x.EmployeeId);
                Correlate(ProfileCreated)
                    .By((saga, message) => true);
                Correlate(FormulaTokenValueChanged)
                    .By((saga, message) => saga.CorrelationId == message.EmployeeId);
                Correlate(ProfileEligibilityEvaluated)
                    .By((saga, message) => saga.CorrelationId == message.PersonId);

                Initially(
                    When(EmployeeCreated)
                        .Then((saga, message) => saga.OnEmployeeCreated(message))
                        .TransitionTo(OnGoing));

                During(OnGoing,
                    When(ProfileCreated)
                        .Then((saga, message) => saga.OnProfileCreated(message)),
                    When(FormulaTokenValueChanged)
                        .Then((saga, message) => saga.OnFormulaTokenValueChanged(message)),
                    When(ProfileEligibilityEvaluated)
                        .Then((saga, message) => saga.OnProfileEligibilityEvaluated(message)));
            });
        }

        public void OnEmployeeCreated(EmployeeCreated message)
        {
            Profiles = new List<PensionSchemeEligibilityProfile>();
        }

        public void OnFormulaTokenValueChanged(IFormulaTokenValueChanged message)
        {
            foreach (var p in Profiles.Where(p => p.EligibilityFormula.Contains(message.NamedVariable)))
            {
                EvaluateProfileEligibility(p);
            }
        }

        public void EvaluateProfileEligibility(PensionSchemeEligibilityProfile profile)
        {
            Bus.Endpoint.Send(new EvaluatePensionEligibilityRequest
                {
                    PersonId = CorrelationId,
                    PensionSchemeId = profile.PensionSchemeIds[0],
                    ProfileId = profile.Id,
                    EligibilityFormula = profile.EligibilityFormula
                });
        }

        public void OnProfileCreated(IProfileCreated message)
        {
            var newProfile = new PensionSchemeEligibilityProfile
                {
                    Id = message.Id,
                    EligibilityFormula = message.EligibilityFormula,
                    PensionSchemeIds = message.PensionSchemeIds
                };

            Profiles.Add(newProfile);

            EvaluateProfileEligibility(newProfile);
        }

        public void OnProfileEligibilityEvaluated(EvaluatePensionEligibilityResponse message)
        {
            var profile = Profiles.Single(x => x.Id == message.ProfileId);

            if (profile.IsEligible == message.IsEligible) 
                return;

            profile.IsEligible = message.IsEligible;

            if (message.IsEligible)
            {
                Bus.Publish(new EmployeeEligibleForPension
                    {
                        EmployeeId = CorrelationId,
                        PensionProfileId = profile.Id,
                        PensionSchemeIds = profile.PensionSchemeIds
                    });
            }
            else
            {
                Bus.Publish(new EmployeeIneligibleForPension
                {
                    EmployeeId = CorrelationId,
                    PensionProfileId = profile.Id,
                    PensionSchemeIds = profile.PensionSchemeIds
                });
            }
        }
    }
}

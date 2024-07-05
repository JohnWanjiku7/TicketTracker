using System.ComponentModel.DataAnnotations;
using TicketTracker.Data.Enums;

namespace TicketTracker.Data
{
    public class Ticket
    {
        [Key]
        public int TicketId { get; private set; }
        public TicketType Type { get; private set; }
        public string Summary { get; private set; }
        public string Description { get; private set; }
        public Severity Severity { get; private set; }
        public Priority Priority { get; private set; }
        public ApplicationUser CreatedBy { get; private set; }
        public Status Status { get; private set; }

        public static Ticket Create(TicketType type, string summary, string description, Severity severity, Priority priority, ApplicationUser createdBy)
        {
            if (createdBy == null)
                throw new ArgumentNullException(nameof(createdBy));

            // Check permissions based on user type
            switch (createdBy.UserType)
            {
                case UserType.QA:
                    if (string.IsNullOrWhiteSpace(summary) || string.IsNullOrWhiteSpace(description))
                        throw new ArgumentException("Summary and Description are required for QA.", string.IsNullOrWhiteSpace(summary) ? nameof(summary) : nameof(description));

                    if (type != TicketType.TestCase && type != TicketType.Bug)
                        throw new InvalidOperationException("QA users can only create test case or bug tickets.");
                    break;

                case UserType.PM:
                    if (type != TicketType.FeatureRequest)
                        throw new InvalidOperationException("PM users can only create feature request tickets.");
                    break;

                case UserType.Administrator:
                    // Administrators can create any type of ticket without restrictions.
                    break;

                default:
                    throw new InvalidOperationException("Only QA, PM, or Administrator users can create tickets.");
            }

            // Create and return the ticket object
            return new Ticket
            {
                Type = type,
                Summary = summary,
                Description = description,
                Severity = severity,
                Priority = priority,
                CreatedBy = createdBy,
                Status = Status.Open
            };
        }




        public void Update(string summary, string description, Severity severity, Priority priority, ApplicationUser editor)
        {
            bool canEdit = editor.UserType == UserType.Administrator ||
                           (editor.UserType == UserType.QA && (Type == TicketType.Bug || Type == TicketType.TestCase)) ||
                           (editor.UserType == UserType.PM && Type == TicketType.FeatureRequest);

            if (!canEdit)
            {
                throw new InvalidOperationException("Only QA users can edit bugs and test cases. Only PM users can edit feature requests.");
            }

            Summary = summary;
            Description = description;
            Severity = severity;
            Priority = priority;
        }

        public void ResolveTicket(ApplicationUser resolver)
        {
            // Allow administrators to resolve any type of ticket
            if (resolver.UserType == UserType.Administrator)
            {
                Status = Status.Resolved;
                return;
            }

            // Check specific permissions for non-administrator users
            if ((Type == TicketType.Bug || Type == TicketType.FeatureRequest) && resolver.UserType != UserType.RD)
            {
                throw new InvalidOperationException("Only RD users can resolve bugs or feature requests.");
            }

            if (Type == TicketType.TestCase && resolver.UserType != UserType.QA)
            {
                throw new InvalidOperationException("Only QA users can resolve test cases.");
            }

            // If permissions are valid, resolve the ticket
            Status = Status.Resolved;
        }
    }
}

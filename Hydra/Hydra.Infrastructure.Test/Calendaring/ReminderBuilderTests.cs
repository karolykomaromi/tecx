namespace Hydra.Infrastructure.Test.Calendaring
{
    using System;
    using Hydra.Infrastructure.Calendaring;
    using Hydra.Infrastructure.Test.Utility;
    using Xunit;
    using Xunit.Extensions;

    public class ReminderBuilderTests
    {
        [Theory, ContainerData]
        public void Should_Build_Reminder_With_Default_Options(ReminderBuilder sut)
        {
            string actual = sut.Build();

            string expected = "BEGIN:VALARM" + Environment.NewLine + 
                              "TRIGGER:-PT15M" + Environment.NewLine +
                              "ACTION:DISPLAY" + Environment.NewLine + 
                              "DESCRIPTION:Reminder" + Environment.NewLine +
                              "END:VALARM";

            Assert.Equal(expected, actual);
        }

        [Theory, ContainerData]
        public void Should_Set_Correct_Time_After_Event(ReminderBuilder sut)
        {
            string actual = sut.RemindAt(x => x.Duration(2.Hours()));

            string expected = "BEGIN:VALARM" + Environment.NewLine +
                              "TRIGGER:PT2H" + Environment.NewLine +
                              "ACTION:DISPLAY" + Environment.NewLine +
                              "DESCRIPTION:Reminder" + Environment.NewLine +
                              "END:VALARM";

            Assert.Equal(expected, actual);
        }

        [Theory, ContainerData]
        public void Should_Set_Correct_Time_Before_Event(ReminderBuilder sut)
        {
            string actual = sut.RemindAt(x => x.Duration(-1.5.Hours()));

            string expected = "BEGIN:VALARM" + Environment.NewLine +
                              "TRIGGER:-PT1H30M" + Environment.NewLine +
                              "ACTION:DISPLAY" + Environment.NewLine +
                              "DESCRIPTION:Reminder" + Environment.NewLine +
                              "END:VALARM";

            Assert.Equal(expected, actual);
        }
    }
}

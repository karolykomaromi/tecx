namespace TecX.EnumClasses.Test.TestObjects
{
    using System.Configuration;

    public class MyConfigSection : ConfigurationSection
    {
        private readonly ConfigurationProperty myEnum;

        public MyConfigSection()
        {
            this.myEnum = new ConfigurationProperty("myEnum", typeof(Numbers), Numbers.None, new EnumClassConverter<Numbers>(), null, ConfigurationPropertyOptions.IsRequired);

            this.Properties.Add(this.myEnum);
        }

        public Numbers MyEnum
        {
            get
            {
                return (Numbers)base[this.myEnum];
            }

            set
            {
                base[this.myEnum] = value;
            }
        }
    }
}
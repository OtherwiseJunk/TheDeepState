namespace DeepState.Models.RPGSystemModels.ElectricBastionland
{
    public class FailedCareer
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string YoungestPlayerDebtor { get; set; }
        public string StartingGear { get; set; }
        public string MoneyQuestion { get; set; }
        public string[] MoneyAnswers { get; set; }
        public string HPQuestion { get;set; }
        public string[] HPAnswers { get; set; }

        public FailedCareer(string name, string description, string youngestPlayerDebtor, string startingGear, string moneyQuestion, string[] moneyAnswers, string hpQuestion, string[] hpAnswers)
        {
            Name = name;
            Description = description;
            YoungestPlayerDebtor = youngestPlayerDebtor;
            StartingGear = startingGear;
            MoneyQuestion = moneyQuestion;
            MoneyAnswers = moneyAnswers;
            HPQuestion = hpQuestion;
            HPAnswers = hpAnswers;
        }
    }
}

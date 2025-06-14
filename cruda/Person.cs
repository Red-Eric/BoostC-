namespace cruda
{
    public class Person
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public int Age { get; set; }
        public string Adresse { get; set; }
        public string Sexe { get; set; }

        public Person() { }

        public Person(string nom, string prenom, int age, string adresse, string sexe)
        {
            Nom = nom;
            Prenom = prenom;
            Age = age;
            Adresse = adresse;
            Sexe = sexe;
        }

        public Person(int id, string nom, string prenom, int age, string adresse, string sexe)
        {
            Id = id;
            Nom = nom;
            Prenom = prenom;
            Age = age;
            Adresse = adresse;
            Sexe = sexe;
        }
    }
}

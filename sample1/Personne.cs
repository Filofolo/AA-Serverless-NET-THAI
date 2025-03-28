namespace sample1;
using Newtonsoft.Json;

class Personne{
    public string nom { get; set; }
    public int age { get; set; }
    public Personne(string nom, int age)
    {
        this.nom = nom;
        this.age = age;
    }
    public string Hello(bool isLowercase)
    {
        if (isLowercase)
            return $"hello {nom}, you are {age}.";
        else
            return $"HELLO {nom.ToUpper()}, YOU ARE {age}.";
    }
}
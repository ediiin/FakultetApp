namespace Fakultet.Core.Modeli
{
    public enum Zvanje
    {
        Docent, 
        RedovniProfesor,
        VanredniProfesor
    }
    public static class ZvanjeEkstenzije
    {
        public static string ToFriendlyString(this Zvanje zvanje)
        {
            return zvanje switch
            {
                Zvanje.Docent => "Docent",
                Zvanje.RedovniProfesor => "Redovni profesor",
                Zvanje.VanredniProfesor => "Vanredni profesor",
                _ => zvanje.ToString()
            };
        }
    }
}

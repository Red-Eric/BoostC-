using System;
using System.Collections.Generic;
using Npgsql;

namespace cruda
{
    public class DataBase
    {
        private readonly string connectionString;

        public DataBase()
        {
            connectionString = "Host=localhost;Port=5432;Database=csharp;Username=postgres;Password=123";
        }

        public void Connect()
        {
            try
            {
                using var conn = new NpgsqlConnection(connectionString);
                conn.Open();
                Console.WriteLine("Connexion réussie à PostgreSQL !");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur de connexion : " + ex.Message);
            }
        }

        public void CreatePersonTable()
        {
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string sql = @"
                CREATE TABLE IF NOT EXISTS person (
                    id SERIAL PRIMARY KEY,
                    nom VARCHAR(100) NOT NULL,
                    prenom VARCHAR(100) NOT NULL,
                    age INT NOT NULL,
                    adresse VARCHAR(200),
                    sexe VARCHAR(20) NOT NULL
                );
            ";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.ExecuteNonQuery();
        }

        public void AddPerson(Person p)
        {
            if (p == null)
                throw new ArgumentNullException(nameof(p));

            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string sql = @"
                INSERT INTO person (nom, prenom, age, adresse, sexe)
                VALUES (@nom, @prenom, @age, @adresse, @sexe);
            ";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("nom", p.Nom);
            cmd.Parameters.AddWithValue("prenom", p.Prenom);
            cmd.Parameters.AddWithValue("age", p.Age);
            cmd.Parameters.AddWithValue("adresse", p.Adresse ?? "");
            cmd.Parameters.AddWithValue("sexe", p.Sexe ?? "");

            cmd.ExecuteNonQuery();
        }

        public List<Person> GetAllPersons()
        {
            var persons = new List<Person>();

            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string sql = "SELECT id, nom, prenom, age, adresse, sexe FROM person;";

            using var cmd = new NpgsqlCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                int id = reader.GetInt32(0);
                string nom = reader.GetString(1);
                string prenom = reader.GetString(2);
                int age = reader.GetInt32(3);
                string adresse = reader.IsDBNull(4) ? "" : reader.GetString(4);
                string sexe = reader.IsDBNull(5) ? "" : reader.GetString(5);

                persons.Add(new Person(id, nom, prenom, age, adresse, sexe));
            }

            return persons;
        }

        public void EditPerson(Person p)
        {
            if (p == null)
                throw new ArgumentNullException(nameof(p));

            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string sql = @"
                UPDATE person SET
                    nom = @nom,
                    prenom = @prenom,
                    age = @age,
                    adresse = @adresse,
                    sexe = @sexe
                WHERE id = @id;
            ";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("nom", p.Nom);
            cmd.Parameters.AddWithValue("prenom", p.Prenom);
            cmd.Parameters.AddWithValue("age", p.Age);
            cmd.Parameters.AddWithValue("adresse", p.Adresse ?? "");
            cmd.Parameters.AddWithValue("sexe", p.Sexe ?? "");
            cmd.Parameters.AddWithValue("id", p.Id);

            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected == 0)
            {
                throw new Exception($"Aucune personne trouvée avec l'id {p.Id} pour la mise à jour.");
            }
        }

        public void DeletePerson(int id)
        {
            using var conn = new NpgsqlConnection(connectionString);
            conn.Open();

            string sql = "DELETE FROM person WHERE id = @id;";

            using var cmd = new NpgsqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("id", id);

            int rowsAffected = cmd.ExecuteNonQuery();
            if (rowsAffected == 0)
            {
                throw new Exception($"Aucune personne trouvée avec l'id {id} pour la suppression.");
            }
        }
    }
}

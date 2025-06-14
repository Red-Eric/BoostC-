using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace cruda
{
    public partial class MainWindow : Window
    {
        private DataBase db = new DataBase();

        public MainWindow()
        {
            InitializeComponent();
            db.Connect();
            db.CreatePersonTable();
            updateAll();
        }

        private void updateAll()
        {
            List<Person> ppl = db.GetAllPersons();
            personDataGrid.ItemsSource = null;
            personDataGrid.ItemsSource = ppl;
        }

        private void BtnAjouterCliquer(object sender, RoutedEventArgs e)
        {
            if (!TryReadFormInputs(out string nom, out string prenom, out int age, out string adresse, out string sexe))
                return;

            Person persTemp = new Person(nom, prenom, age, adresse, sexe);
            db.AddPerson(persTemp);
            MessageBox.Show("Personne ajoutée avec succès !");
            ClearForm();
            updateAll();
        }

        private void BtnModifierCliquer(object sender, RoutedEventArgs e)
        {
            if (personDataGrid.SelectedItem is not Person selected)
            {
                MessageBox.Show("Veuillez sélectionner une personne à modifier.");
                return;
            }

            if (!TryReadFormInputs(out string nom, out string prenom, out int age, out string adresse, out string sexe))
                return;

            selected.Nom = nom;
            selected.Prenom = prenom;
            selected.Age = age;
            selected.Adresse = adresse;
            selected.Sexe = sexe;


            db.EditPerson(selected);
            MessageBox.Show("Personne modifiée !");
            updateAll();
        }

        private void BtnSupprimerCliquer(object sender, RoutedEventArgs e)
        {
            if (personDataGrid.SelectedItem is not Person selected)
            {
                MessageBox.Show("Veuillez sélectionner une personne à supprimer.");
                return;
            }

            db.DeletePerson(selected.Id);
            MessageBox.Show("Personne supprimée !");
            updateAll();
        }

        private bool TryReadFormInputs(out string nom, out string prenom, out int age, out string adresse, out string sexe)
        {
            nom = nomAjouter.Text.Trim();
            prenom = prenomAjouter.Text.Trim();
            adresse = adresseAjouter.Text.Trim();
            sexe = (sexeAjouter.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "";
            string ageText = ageAjouter.Text.Trim();

            if (string.IsNullOrEmpty(nom) || string.IsNullOrEmpty(prenom) ||
                string.IsNullOrEmpty(adresse) || string.IsNullOrEmpty(sexe))
            {
                MessageBox.Show("Tous les champs sont obligatoires.");
                age = 0;
                return false;
            }

            if (!int.TryParse(ageText, out age) || age < 0 || age > 150)
            {
                MessageBox.Show("Âge invalide (0-150).");
                return false;
            }

            return true;
        }

        private void ClearForm()
        {
            nomAjouter.Clear();
            prenomAjouter.Clear();
            ageAjouter.Clear();
            adresseAjouter.Clear();
            sexeAjouter.SelectedIndex = -1;
        }
    }
}

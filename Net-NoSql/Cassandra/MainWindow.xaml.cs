using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Cassandra
{
    /// <summary>
    /// Lógica de interacción para MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Cluster _cluster;
        ISession _session;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _cluster = Cluster.Builder().AddContactPoint("127.0.0.1").Build();
                _session = _cluster.Connect("demo");
                EscribirEnConsola("Conexión establecida correctamente");
            }
            catch(Exception ex)
            {
                EscribirEnConsola("Error al conectar con Cassandra. \n" + ex.Message);
            }
            
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Inserta un registro en la tabla tweets
            var insert = "INSERT INTO tweets(user, time_posted, tweet) " +
                         "   VALUES('Asier 2', '2015-10-25 12:34:27', 'Mi primer tweet')";
            var res = _session.Execute(insert);
            if (res.IsFullyFetched)
                EscribirEnConsola("Registro añadido correctamente");
            else
                EscribirEnConsola("Error al insertar el registro");
        }

        private void EscribirEnConsola(string text)
        {
            rtbResults.Document.Blocks.Add(new Paragraph(new Run(text)));
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            var rows = _session.Execute("select * from tweets");
            foreach (Row row in rows)
                EscribirEnConsola(string.Format("{0} {1} {2}", 
                                    row["user"], row["time_posted"], row["tweet"]));
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            // Actualiza un registro en la tabla tweets
            var update = "update tweets set tweet='Mi primer tweet actualizado'" +
                         " where user='Asier' and time_posted='2015-10-25 11:34:27'";
            var res = _session.Execute(update);
            if (res.IsFullyFetched)
                EscribirEnConsola("Registro actualizado correctamente");
            else
                EscribirEnConsola("Error al actualizar el registro");
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            // Borra un registro en la tabla tweets
            var delete = "delete from tweets where user = 'Asier' " + 
                         "  and time_posted='2015-10-25 11:34:27'";
            var res = _session.Execute(delete);
            if (res.IsFullyFetched)
                EscribirEnConsola("Registro eliminado correctamente");
            else
                EscribirEnConsola("Error al eliminar el registro");
        }
    }
}

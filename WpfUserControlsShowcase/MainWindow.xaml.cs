using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading;
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
using WpfUserControlLibrary;

namespace WpfUserControlsShowcase
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void SelfMainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            Rows.Add(new Row("999 222,00", GetOutput));
            Rows.Add(new Row("322 444 666,88", GetOutput));
            Rows.Add(new Row("465 444 666,99", GetOutput));
            Rows.Add(new Row("282 222,99", GetOutput));
            Rows.Add(new Row("666 555 888", GetOutput));
            Rows.Add(new Row("322 444 666,88", GetOutput));
            Rows.Add(new Row("222 212,99", GetOutput));
            Rows.Add(new Row("999 555 888", GetOutput));
            Rows.Add(new Row(GetOutput));
            Rows.Add(new Row(GetOutput));
            Rows.Add(new Row());
            Rows.Add(new Row());
        }

        public static DependencyProperty GetOutputProperty =
            DependencyProperty.Register("GetOutput",
                typeof(Func<string, Task<string>>),
                typeof(MainWindow),
                new PropertyMetadata(new Func<string, Task<string>>(async (s) => await Task.Run(
                    () => { var r = s + " " + s + " " + s; Thread.Sleep(1000); return r; }
        ))));
        public Func<string, Task<string>> GetOutput
        {
            get { return (Func<string, Task<string>>)GetValue(GetOutputProperty); }
            set { SetValue(GetOutputProperty, value); }
        }

        private static Func<string, Task<string>> GetOutputTemp = async (s) => await Task.Run(
                () => { var r = s + " " + s + " " + s; Thread.Sleep(1000); return r; }
        );

        public static DependencyProperty RowsProperty =
            DependencyProperty.Register("Rows",
            typeof(ICollection<Row>),
            typeof(TriggerListAdd),
            new PropertyMetadata(new ObservableCollection<Row>()));
        public ICollection<Row> Rows
        {
            get { return (ICollection<Row>)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }
    }
}
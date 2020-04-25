using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace WpfUserControlLibrary
{
    public partial class TriggerListAdd : UserControl
    {
        public TriggerListAdd()
        {
            InitializeComponent();
        }

        public static DependencyProperty GetOutputFromInputProperty =
            DependencyProperty.Register("GetOutputFromInput", 
                typeof(Func<string, Task<string>>), 
                typeof(TriggerListAdd));
        public Func<string, Task<string>> GetOutputFromInput
        {
            get { return (Func<string, Task<string>>)GetValue(GetOutputFromInputProperty); }
            set { SetValue(GetOutputFromInputProperty, value); }
        }

        public static DependencyProperty RowsProperty =
            DependencyProperty.Register("ControlRows",
                typeof(ICollection<Row>),
                typeof(TriggerListAdd));
        public ICollection<Row> ControlRows
        {
            get { return (ICollection<Row>)GetValue(RowsProperty); }
            set { SetValue(RowsProperty, value); }
        }

        public static DependencyProperty TextableProperty =
            DependencyProperty.Register("Textable",
                typeof(string),
                typeof(TriggerListAdd));
        public string Textable
        {
            get { return (string)GetValue(TextableProperty); }
            set { SetValue(TextableProperty, value); }
        }

        private void ButtonGetOutput_Click(object sender, RoutedEventArgs e)
        {
            ControlRows.Add(new Row(TextboxInput.Text, GetOutputFromInput));
        }

        private void SelfTriggerListAdd_Loaded(object sender, RoutedEventArgs e)
        {
            ControlRows.Add(new Row("999 222,00", GetOutputFromInput));
            ControlRows.Add(new Row("322 444 666,88", GetOutputFromInput));
            ControlRows.Add(new Row("465 444 666,99", GetOutputFromInput));
            ControlRows.Add(new Row("282 222,99", GetOutputFromInput));
            ControlRows.Add(new Row("666 555 888", GetOutputFromInput));
            ControlRows.Add(new Row("322 444 666,88", GetOutputFromInput));
            ControlRows.Add(new Row("222 212,99", GetOutputFromInput));
            ControlRows.Add(new Row("999 555 888", GetOutputFromInput));
            ControlRows.Add(new Row(GetOutputFromInput));
            ControlRows.Add(new Row(GetOutputFromInput));
            ControlRows.Add(new Row());
            ControlRows.Add(new Row());
        }
    }

    public class Row : INotifyPropertyChanged
    {
        public Row(string input, Func<string, Task<string>> outputFunc)
        {
            _getOutputFromInput = outputFunc;
            _input = input;
        }

        public Row() { }

        public Row(Func<string, Task<string>> outputFunc)
        {
            _getOutputFromInput = outputFunc;
        }

        public string Input
        {
            get
            {
                return _input;
            }

            set
            {
                if (_input != value)
                {
                    _input = value;
                    RaisePropertyChanged("Input");
                    RaisePropertyChanged("Output");
                }
            }
        }

        private readonly Func<string, Task<string>> _getOutputFromInput;

        public string Output
        {
            get
            {
                if (_getOutputFromInput != null)
                    _getOutputFromInput(_input).ContinueWith((t) => { _output = t.Result; RaisePropertyChanged("Output"); });
                return _output;
            }
            set
            {
                _output = value;
                RaisePropertyChanged("Output");
            }
        }

        private string _input;
        private string _output;

        public event PropertyChangedEventHandler PropertyChanged;

        private void RaisePropertyChanged(string property)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }
}

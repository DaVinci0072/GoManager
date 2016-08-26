using PokemonGoGUI.AccountScheduler;
using PokemonGoGUI.Enums;
using System;
using System.Drawing;
using System.Windows.Forms;
using PokemonGoGUI.Extensions;

namespace PokemonGoGUI.UI
{
    public partial class SchedulerSettingForm : Form
    {
        private Scheduler _scheduler;
        private Color _color;

        public SchedulerSettingForm(Scheduler scheduler)
        {
            InitializeComponent();

            _scheduler = scheduler;

            foreach(SchedulerOption option in Enum.GetValues(typeof(SchedulerOption)))
            {
                if (option != SchedulerOption.DisableEnable)
                {
                    comboBoxMasterAction.Items.Add(option);
                }

                comboBoxPokemonAction.Items.Add(option);
                comboBoxPokestopAction.Items.Add(option);
            }

            textBoxChosenColor.BackColor = Color.FromArgb(43, 43, 43);
        }

        private void SchedulerSettingForm_Load(object sender, EventArgs e)
        {
            UpdateDetails();
        }

        private void UpdateDetails()
        {
            Text = _scheduler.Name;

            _color = _scheduler.NameColor;

            textBoxChosenColor.ForeColor = _color;

            textBoxName.Text = _scheduler.Name;

            numericUpDownCheckSpeed.Value = _scheduler.CheckTime;

            comboBoxPokemonAction.SelectOption<SchedulerOption>(_scheduler.PokemonLimiter.Option);
            numericUpDownPokemonMin.Value = _scheduler.PokemonLimiter.Min;
            numericUpDownPokemonMax.Value = _scheduler.PokemonLimiter.Max;

            comboBoxPokestopAction.SelectOption<SchedulerOption>(_scheduler.PokeStoplimiter.Option);
            numericUpDownPokestopsMin.Value = _scheduler.PokeStoplimiter.Min;
            numericUpDownPokestopsMax.Value = _scheduler.PokeStoplimiter.Max;

            comboBoxMasterAction.SelectOption<SchedulerOption>(_scheduler.MasterOption);


            //New time pickers ...
            TimeSpan startTime = TimeSpan.FromHours(_scheduler.StartTime);
            TimeSpan endTime = TimeSpan.FromHours(_scheduler.EndTime);

            timePickerStartTime.Hours = startTime.Hours;
            timePickerStartTime.Minutes = startTime.Minutes;

            timePickerEndTime.Hours = endTime.Hours;
            timePickerEndTime.Minutes = endTime.Minutes;
        }

        private bool SaveValues()
        {
            if(comboBoxMasterAction.HasNullItem() || comboBoxPokemonAction.HasNullItem() || comboBoxPokestopAction.HasNullItem())
            {
                MessageBox.Show("Please select valid options for the actions", "Warning");
                return false;
            }

            _scheduler.Name = textBoxName.Text;

            _scheduler.StartTime = new TimeSpan(timePickerStartTime.Hours, timePickerStartTime.Minutes, 0).TotalHours;
            _scheduler.EndTime = new TimeSpan(timePickerEndTime.Hours, timePickerEndTime.Minutes, 0).TotalHours;
            _scheduler.CheckTime = (int)numericUpDownCheckSpeed.Value;

            _scheduler.PokemonLimiter.Option = (SchedulerOption)comboBoxPokemonAction.SelectedItem;
            _scheduler.PokemonLimiter.Min = (int)numericUpDownPokemonMin.Value;
            _scheduler.PokemonLimiter.Max = (int)numericUpDownPokemonMax.Value;

            _scheduler.PokeStoplimiter.Option = (SchedulerOption)comboBoxPokestopAction.SelectedItem;
            _scheduler.PokeStoplimiter.Min = (int)numericUpDownPokestopsMin.Value;
            _scheduler.PokeStoplimiter.Max = (int)numericUpDownPokestopsMax.Value;

            _scheduler.MasterOption = (SchedulerOption)comboBoxMasterAction.SelectedItem;

            _scheduler.NameColor = _color;

            return true;
        }

        private void buttonDone_Click(object sender, EventArgs e)
        {
            if(SaveValues())
            {
                DialogResult = DialogResult.OK;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if(colorDialogNameColor.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            _color = colorDialogNameColor.Color;
            textBoxChosenColor.ForeColor = _color;
        }

        private void timePickerStartTime_OnValueChanged(object sender, EventArgs e)
        {

        }

        private void timePickerEndTime_OnValueChanged(object sender, EventArgs e)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows;
using Layer_2_Common.Type;
using System.Windows.Controls;
using Layer_UI.Satis.Sevk;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace Layer_UI.Methods
{
    public class UIinteractions
    {
        public static T FindVisualParent<T>(DependencyObject obj) where T : DependencyObject
        {
            while (obj != null)
            {
                if (obj is T parent)
                {
                    return parent;
                }
                obj = VisualTreeHelper.GetParent(obj);
            }
            return null;
        }

        public static T FindSpecificForm<T>() where T : Window
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is T)
                {
                    return (T)window;
                }
            }

            return null;
        }

		public static T GetDataItemFromButton<T>(object sender) where T : class
		{
			T dataItem = null;

			Button button = sender as Button;

			if (button == null)
			{
				MessageBox.Show("Hata ile Karşılaşıldı.");
				return null;
			}

			DataGridRow row = FindVisualParent<DataGridRow>(button);

			if (row == null)
			{
				MessageBox.Show("Hata ile Karşılaşıldı.");
				return null;
			}

			dataItem = row.Item as T;

			if (dataItem == null)
			{
				MessageBox.Show("Hata ile Karşılaşıldı.");
				return null;
			}

			return dataItem;
		}

        public static bool IsNumeric(char c)
        {
            return char.IsDigit(c);
        }

        public static readonly DependencyProperty AllowNumberInputProperty =
       DependencyProperty.RegisterAttached(
           "AllowNumberInput",
           typeof(bool),
           typeof(UIinteractions),
           new PropertyMetadata(false, OnAllowNumberInputChanged)
       );
        public static readonly DependencyProperty AllowFloatInputProperty =
       DependencyProperty.RegisterAttached(
           "AllowFloatInput",
           typeof(bool),
           typeof(UIinteractions),
           new PropertyMetadata(false, OnAllowFloatInputChanged)
       );

        public static bool GetAllowNumberInput(DependencyObject obj)
        {
            return (bool)obj.GetValue(AllowNumberInputProperty);
        }

        public static void SetAllowNumberInput(DependencyObject obj, bool value)
        {
            obj.SetValue(AllowNumberInputProperty, value);
        }
        public static bool GetAllowFloatInput(DependencyObject obj)
        {
            return (bool)obj.GetValue(AllowFloatInputProperty);
        }

        public static void SetAllowFloatInput(DependencyObject obj, bool value)
        {
            obj.SetValue(AllowFloatInputProperty, value);
        }

        private static void OnAllowFloatInputChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                if ((bool)e.NewValue)
                {
                    textBox.PreviewTextInput += FloatOnlyPreviewTextInput;
                }
                else
                {
                    textBox.PreviewTextInput -= FloatOnlyPreviewTextInput;
                }
            }

            if (d is DataGridTextColumn dataGridTextColumn)
            {
                if ((bool)e.NewValue)
                {
                    dataGridTextColumn.EditingElementStyle = GetEditingElementStyleFloat();
                }
            }
        }
        private static void OnAllowNumberInputChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is TextBox textBox)
            {
                if ((bool)e.NewValue)
                {
                    textBox.PreviewTextInput += NumberOnlyPreviewTextInput;
                }
                else
                {
                    textBox.PreviewTextInput -= NumberOnlyPreviewTextInput;
                }
            }

            if (d is DataGridTextColumn dataGridTextColumn)
            {
                if ((bool)e.NewValue)
                {
                    dataGridTextColumn.EditingElementStyle = GetEditingElementStyle();
                }
            }
        }

        private static void NumberOnlyPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;

            if (!IsNumberInput(e.Text))
            {
                e.Handled = true;
            }
        }
        private static void FloatOnlyPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var textBox = sender as TextBox;

            if (!IsFloatInput(e.Text))
            {
                e.Handled = true;
            }
        }

        private static bool IsNumberInput(string input)
        {
            return Regex.IsMatch(input, @"^[0-9]+$");
        }
        private static bool IsFloatInput(string input)
        {
            string floatPattern = @"^[0-9.]$";

            return Regex.IsMatch(input, floatPattern);
        }

        private static Style GetEditingElementStyle()
        {
            var style = new Style(typeof(TextBox));

            style.Setters.Add(new EventSetter(TextBox.PreviewTextInputEvent, new TextCompositionEventHandler(NumberOnlyPreviewTextInput)));

            return style;
        }
        private static Style GetEditingElementStyleFloat()
        {
            var style = new Style(typeof(TextBox));

            style.Setters.Add(new EventSetter(TextBox.PreviewTextInputEvent, new TextCompositionEventHandler(FloatOnlyPreviewTextInput)));

            return style;
        }

    }
}


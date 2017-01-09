using System;
using System.Text.RegularExpressions;
using Xamarin.Forms;

namespace Solum.Behaviors
{
    public class EmailValidatorBehavior : Behavior<Entry>
    {
        private const string emailRegex =
            @"^(?("")("".+?(?<!\\)""@)|(([0-9a-z]((\.(?!\.))|[-!#\$%&'\*\+/=\?\^`\{\}\|~\w])*)(?<=[0-9a-z])@))" +
            @"(?(\[)(\[(\d{1,3}\.){3}\d{1,3}\])|(([0-9a-z][-\w]*[0-9a-z]*\.)+[a-z0-9][\-a-z0-9]{0,22}[a-z0-9]))$";

        private static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid",
            typeof(bool), typeof(EmailValidatorBehavior), false);

        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

        public bool IsValid
        {
            get { return (bool) GetValue(IsValidProperty); }
            private set { SetValue(IsValidPropertyKey, value); }
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += OnTextChanged;
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= OnTextChanged;
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            IsValid = Regex.IsMatch(e.NewTextValue, emailRegex, RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
            ((Entry) sender).TextColor = IsValid ? Color.Green : Color.Red;
        }
    }

    public class RequiredValidatorBehavior : Behavior<Entry>
    {
        private static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid",
            typeof(bool), typeof(RequiredValidatorBehavior), false);

        private const string PasswordRegex = @"^(?=.*[A-Z])(?=.*[!@#$&*])(?=.*[0-9])(?=.*[a-z]).{8,15}$";

        private static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

        public bool IsValid
        {
            get { return (bool) GetValue(IsValidProperty); }
            private set { SetValue(IsValidPropertyKey, value); }
        }

        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += OnTextChanged;
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= OnTextChanged;
            base.OnDetachingFrom(bindable);
        }

        private void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            IsValid = Regex.IsMatch(e.NewTextValue, PasswordRegex, RegexOptions.None, TimeSpan.FromMilliseconds(300));
            ((Entry) sender).TextColor = IsValid ? Color.Green : Color.Red;
        }
    }

    public class ConfirmPasswordBehavior : Behavior<Entry>
    {
        static readonly BindablePropertyKey IsValidPropertyKey = BindableProperty.CreateReadOnly("IsValid", typeof(bool), typeof(ConfirmPasswordBehavior), false);
        public static readonly BindableProperty IsValidProperty = IsValidPropertyKey.BindableProperty;

        public static readonly BindableProperty CompareToEntryProperty = BindableProperty.Create("CompareToEntry", typeof(Entry), typeof(ConfirmPasswordBehavior), null);

        public Entry CompareToEntry
        {
            get { return (Entry)base.GetValue(CompareToEntryProperty); }
            set { base.SetValue(CompareToEntryProperty, value); }
        }
        public bool IsValid
        {
            get { return (bool)base.GetValue(IsValidProperty); }
            private set { base.SetValue(IsValidPropertyKey, value); }
        }
        protected override void OnAttachedTo(Entry bindable)
        {
            bindable.TextChanged += OnTextChanged;
            base.OnAttachedTo(bindable);
        }
        protected override void OnDetachingFrom(Entry bindable)
        {
            bindable.TextChanged -= OnTextChanged;
            base.OnDetachingFrom(bindable);
        }
        void OnTextChanged(object sender, TextChangedEventArgs e)
        {
            var password = CompareToEntry.Text;
            var confirmPassword = e.NewTextValue;
            IsValid = password.Equals(confirmPassword);
            ((Entry)sender).TextColor = IsValid ? Color.Green : Color.Red;
        }
    }
}
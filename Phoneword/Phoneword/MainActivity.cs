using Android.App;
using Android.Content;
using Android.OS;
using Android.Widget;
using static System.String;
using Uri = Android.Net.Uri;

namespace Phoneword
{
    [Activity(Label = "Phone Word", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            var phoneNumberText = FindViewById<EditText>(Resource.Id.PhoneNumberText);
            var translateButton = FindViewById<Button>(Resource.Id.TranslateButton);
            var callButton = FindViewById<Button>(Resource.Id.CallButton);

            callButton.Enabled = false;

            var translatedNumber = Empty;

            translateButton.Click += (sender, e) =>
            {
                translatedNumber = PhoneTranslator.ToNumber(phoneNumberText.Text);

                if (IsNullOrWhiteSpace(translatedNumber))
                {
                    callButton.Text = "Call";
                    callButton.Enabled = false;
                }
                else
                {
                    callButton.Text = $"Call #{translatedNumber}";
                    callButton.Enabled = true;
                }
            };

            callButton.Click += (sender, args) =>
            {
                var callDialog = new AlertDialog.Builder(this);

                callDialog.SetMessage($"Call #{translatedNumber}?");

                callDialog.SetNeutralButton("Call", (o, eventArgs) =>
                {
                    var callIntent = new Intent(Intent.ActionCall);

                    callIntent.SetData(Uri.Parse("tel:" + translatedNumber));

                    StartActivity(callIntent);
                });

                callDialog.SetNegativeButton("Cancel", (o, eventArgs) => { });

                callDialog.Show();
            };
        }
    }
}


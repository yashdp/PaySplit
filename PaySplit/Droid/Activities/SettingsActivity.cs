﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Preferences;
using Android.Runtime;
using Android.Views;
using Android.Widget;

#pragma warning disable CS0618 // Type or member is obsolete
namespace PaySplit.Droid
{
	[Activity(Label = "Settings")]
	public class SettingsActivity : PreferenceActivity, ISharedPreferencesOnSharedPreferenceChangeListener
	{

		private GenDataService mDBS;
        private CheckBoxPreference cbPref;
        private PreferenceCategory budgetPref;
        private PreferenceScreen catPref;

        protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			this.AddPreferencesFromResource(Resource.Xml.preferences_settings);
			PreferenceManager.SetDefaultValues(this, Resource.Xml.preferences_settings, true);

			mDBS = DataHelper.getInstance().getGenDataService();

			try
			{
				Contact c = mDBS.getUserContactInformation();
				Settings.SetDefaultName(this, c.FullName);
				FindPreference(GetString(Resource.String.pref_update_name)).Summary = c.FullName;

				Settings.SetDefaultName(this, c.Email);
				FindPreference(GetString(Resource.String.pref_update_email)).Summary = c.Email;

                cbPref = (CheckBoxPreference)FindPreference(GetString(Resource.String.pref_enable_insights));
                budgetPref = (PreferenceCategory)FindPreference(GetString(Resource.String.pref_budgeting_category));
                if (catPref == null)
                {
                    catPref = (PreferenceScreen)FindPreference(GetString(Resource.String.pref_insights_categories));
                }

                showInsights();

            }
			catch (Exception)
			{
				Toast.MakeText(this, "Unable to get contact", ToastLength.Short).Show();
				// Unable to fetch contact and update fields
			}
		}

		public void OnSharedPreferenceChanged(ISharedPreferences sharedPreferences, string key)
		{
			if (key == null)
			{
				return;
			}

			Preference pref = FindPreference(key);
			if (key.Equals(GetString(Resource.String.pref_update_name)))
			{
				EditTextPreference etp = (EditTextPreference)pref;
				pref.Summary = etp.Text;

				Contact c = mDBS.getUserContactInformation();
				c.FullName = etp.Text;

				mDBS.UpdateUserContactInformation(c);
			}
			else if (key.Equals(GetString(Resource.String.pref_update_email)))
			{
				EditTextPreference etp = (EditTextPreference)pref;
				pref.Summary = etp.Text;

				Contact c = mDBS.getUserContactInformation();
				c.Email = etp.Text;

				mDBS.UpdateUserContactInformation(c);
			} else if (key.Equals(GetString(Resource.String.pref_enable_insights)))
            {
                showInsights();
            }
			else
			{
                //Toast.MakeText(this, key, ToastLength.Short).Show();
                EditTextPreference etp = (EditTextPreference)pref;
                pref.Summary = etp.Text;
            }
		}

        private void showInsights()
        {
            if (!cbPref.Checked)
            {
                if (budgetPref != null && catPref != null)
                {
                    budgetPref.RemovePreference(catPref);
                }
            }
            else
            {
                if (budgetPref != null)
                {
                    budgetPref.AddPreference(catPref);
                    FindPreference(GetString(Resource.String.pref_food_drink)).Summary = ((EditTextPreference)FindPreference(GetString(Resource.String.pref_food_drink))).Text;
                    FindPreference(GetString(Resource.String.pref_entertainment)).Summary = ((EditTextPreference)FindPreference(GetString(Resource.String.pref_entertainment))).Text;
                    FindPreference(GetString(Resource.String.pref_utilities)).Summary = ((EditTextPreference)FindPreference(GetString(Resource.String.pref_utilities))).Text;
                    FindPreference(GetString(Resource.String.pref_education)).Summary = ((EditTextPreference)FindPreference(GetString(Resource.String.pref_education))).Text;
                    FindPreference(GetString(Resource.String.pref_personal_care)).Summary = ((EditTextPreference)FindPreference(GetString(Resource.String.pref_personal_care))).Text;
                    FindPreference(GetString(Resource.String.pref_shopping)).Summary = ((EditTextPreference)FindPreference(GetString(Resource.String.pref_shopping))).Text;
                    FindPreference(GetString(Resource.String.pref_work)).Summary = ((EditTextPreference)FindPreference(GetString(Resource.String.pref_work))).Text;
                    FindPreference(GetString(Resource.String.pref_travel)).Summary = ((EditTextPreference)FindPreference(GetString(Resource.String.pref_travel))).Text;
                    FindPreference(GetString(Resource.String.pref_transportation)).Summary = ((EditTextPreference)FindPreference(GetString(Resource.String.pref_transportation))).Text;
                    FindPreference(GetString(Resource.String.pref_charity)).Summary = ((EditTextPreference)FindPreference(GetString(Resource.String.pref_charity))).Text;
                    FindPreference(GetString(Resource.String.pref_investment)).Summary = ((EditTextPreference)FindPreference(GetString(Resource.String.pref_investment))).Text;
                    FindPreference(GetString(Resource.String.pref_other)).Summary = ((EditTextPreference)FindPreference(GetString(Resource.String.pref_other))).Text;
                    budgetPref.AddPreference(catPref);
                }
            }
        }

		protected override void OnPause()
		{
			base.OnPause();
			if (this.PreferenceManager != null)
			{
				this.PreferenceManager.SharedPreferences.UnregisterOnSharedPreferenceChangeListener(this);
			}
		}

		protected override void OnResume()
		{
			base.OnResume();
			if (this.PreferenceManager != null)
			{
				this.PreferenceManager.SharedPreferences.RegisterOnSharedPreferenceChangeListener(this);
			}
		}

	}
}
#pragma warning disable CS0618 // Type or member is obsolete
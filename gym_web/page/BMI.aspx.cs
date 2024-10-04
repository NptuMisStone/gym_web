using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class page_BMI : System.Web.UI.Page
{
    double weight, height,BMI;
    int age,gender;
    string result;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Panel1.Visible = false;
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        gender = Gender.SelectedIndex;
        if (double.TryParse(Weight.Text, out weight) && double.TryParse(Height.Text, out height)
            && int.TryParse(Age.Text, out age) && Gender.SelectedIndex!=0)
        {
            BMI = weight / ((height/100) * (height/100));
            
            if (gender == 1)
            {
                result = GetBMICategoryForMale(BMI, age);
            }
            else if(gender == 2)
            {
                result = GetBMICategoryForFemale(BMI, age);
            }

            Label1.Text = $"你的BMI為：{BMI:F1}{result}";
            Panel1.Visible = true;
        }
        else
        {
            Label1.Text = "請輸入有效的身高、體重、年齡及性別！";
        }
    }
    private string GetBMICategoryForMale(double bmi, int age)
    {
        if (age < 18)
        {
            if (bmi < 18.5) return "(過輕)";
            else if (bmi < 24.9) return "(正常)";
            else if (bmi < 29.9) return "(超重)";
            else return "(肥胖)";
        }
        else
        {
            if (bmi < 20) return "(過輕)";
            else if (bmi < 25) return "(正常)";
            else if (bmi < 30) return "(超重)";
            else return "(肥胖)";
        }
    }

    private string GetBMICategoryForFemale(double bmi, int age)
    {
        if (age < 18)
        {
            if (bmi < 18.5) return "(過輕)";
            else if (bmi < 24.9) return "(正常)";
            else if (bmi < 29.9) return "(超重)";
            else return "(肥胖)";
        }
        else
        {
            if (bmi < 19) return "(過輕)";
            else if (bmi < 24) return "(正常)";
            else if (bmi < 29) return "(超重)";
            else return "(肥胖)";
        }
    }


}
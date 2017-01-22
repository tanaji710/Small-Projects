using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Diagnostics;

namespace calculator_web
{
    public partial class Main : System.Web.UI.Page
    {

        private enum CalculatorStateType
        {
            Zero, NumberWithoutPoint, NumberWithPoint, Operator, Finish
        };

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CurrentState = CalculatorStateType.Zero;
            }

            for (int i = 0; i <= 9; i++)
            {
                (FindControl("btnNum" + i) as Button).Click += btnNumStar_Click;
            }

            btnPlus.Click += btnOperator_Click;
            btnMinus.Click += btnOperator_Click;
            btnMutiply.Click += btnOperator_Click;
            btnDivide.Click += btnOperator_Click;
        }

        

        
        private void btnOperator_Click(object sender, EventArgs e)
        {
            //Debug.WriteLine("operator");
            Button btnSender = sender as Button;

            if (CurrentState == CalculatorStateType.Zero ||
                CurrentState == CalculatorStateType.NumberWithPoint ||
                CurrentState == CalculatorStateType.NumberWithoutPoint)
            {
                FirstOperand = double.Parse(lblDisplay.Text);
                Operator = char.Parse(btnSender.Text);

                CurrentState = CalculatorStateType.Operator;
            }
            else if (
                CurrentState == CalculatorStateType.Finish || 
                CurrentState == CalculatorStateType.Zero)
            {
                return;
            }
        }


        private void btnNumStar_Click(object sender, EventArgs e)
        {
            Button btnSender = sender as Button;

            if (CurrentState == CalculatorStateType.Zero || 
                CurrentState == CalculatorStateType.Operator ||
                CurrentState == CalculatorStateType.Finish)
            {
                lblDisplay.Text = btnSender.Text;

                if (btnSender.Text != "0")
                {
                    CurrentState = CalculatorStateType.NumberWithoutPoint;
                }
            }
            else if (
                CurrentState == CalculatorStateType.NumberWithoutPoint ||
                CurrentState == CalculatorStateType.NumberWithPoint)
            {
                lblDisplay.Text += btnSender.Text;
            }
        }

        protected void btnEqual_Click(object sender, EventArgs e)
        {
            if (CurrentState == CalculatorStateType.Finish ||
                CurrentState == CalculatorStateType.Operator)
            {
                return;
            }
            else if (
                CurrentState == CalculatorStateType.Zero ||
                CurrentState == CalculatorStateType.NumberWithPoint ||
                CurrentState == CalculatorStateType.NumberWithoutPoint)
            {
                if (FirstOperand != null && Operator != null)
                {
                    SecondOperand = double.Parse(lblDisplay.Text);
                    lblDisplay.Text = string.Format("{0:G}", Calculate(FirstOperand.Value, SecondOperand.Value, Operator.Value));

                    CurrentState = CalculatorStateType.Finish;
                    FirstOperand = null;
                    Operator = null;
                    SecondOperand = null;
                }
            }
        }

        protected void btnPoint_Click(object sender, EventArgs e)
        {
            if (CurrentState == CalculatorStateType.Finish ||
                CurrentState == CalculatorStateType.NumberWithPoint ||
                CurrentState == CalculatorStateType.Operator)
            {
                return;
            }
            else if (
                CurrentState == CalculatorStateType.Zero ||
                CurrentState == CalculatorStateType.NumberWithoutPoint
                )
            {
                lblDisplay.Text += ".";

                CurrentState = CalculatorStateType.NumberWithPoint;
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            CurrentState = CalculatorStateType.Zero;
            FirstOperand = null;
            SecondOperand = null;
            Operator = null;
            lblDisplay.Text = "0";
        }

        #endregion

        private double Calculate(double a, double b, char op)
        {
            switch (op)
            {
                case '+':
                    return a + b;
                case '-':
                    return a - b;
                case '*':
                    return a * b;
                case '/':
                    if (b != 0) return a / b;
                    else return 0;
            }
            return 0;
        }
    }
}
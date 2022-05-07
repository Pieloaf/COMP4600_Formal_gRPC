using System;
using System.Windows.Forms;
using CalculatorClient;

// using CalculatorGRPC namespace
namespace CalculatorGRPC
{
    // Windows forms object
    public partial class gRPCalculatorForm : Form
    {
        // variable initialisation
        private double runningTotal = 0; 
        private string previousOp = "+";
        private bool firstFlag = true;
        private bool rpcError = false;
        private bool deg = true;

        // instantiating the TrigCalcualtor class from the CalculatorClient DLL
        private TrigCalculator grpcCalc = new TrigCalculator(); 

        // Form Constructor
        public gRPCalculatorForm()
        {
            InitializeComponent();
            grpcCalc.CreateChannel(); // create a gRPC channel on Form construction
        }

        private async void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // close gRPC channel on Form closing
            await grpcCalc.CloseChannel();
        }

        // Number Input handler 
        private void numberInput(string data)
        {
            // if number is first input after operation or startup
            if (firstFlag)
            {
                textBox1.Text = ""; // clear screen
                firstFlag = false;  // set firstFlag false
            }

            // if the last input was an rpcError
            if (rpcError)
            {
                // clear the flag and continue as normal
                rpcError = false;
            }
            // add data to textbox
            textBox1.Text += data;
        }

        // Maths Function Input handler 
        private void functionInput(string data)
        {
            // set temporary variable to 0
            double temp = 0;

            // if rpcError reset flag and leave temp set to 0
            if (rpcError) { rpcError = false; }
            // else set temp to value in text box
            else { temp = Convert.ToDouble(textBox1.Text); }

            // switch statement to handle various math functions and update the running total
            // The switch statement carries out the previously entered operation
            switch (previousOp)
            {
                case "+":
                    runningTotal += temp;
                    break;
                case "-":
                    runningTotal -= temp;
                    break;
                case "×":
                    runningTotal *= temp;
                    break;
                case "÷":
                    runningTotal /= temp;
                    break;
                case "√x":
                    runningTotal = Math.Sqrt(temp);
                    break;
                case "%":
                    runningTotal = runningTotal/100*temp;
                    break;
                case "xʸ":
                    runningTotal = Math.Pow(runningTotal, temp);
                    break;
            }
            // the current operation is then saved to be executed on the next function input
            previousOp = data;
            // add the new running total to the text box
            textBox1.Text = Convert.ToString(runningTotal);
            firstFlag = true; // set first flag for new number inputs
        }

        // helper function to handle the gRPC repsonse objects
        private void handleTrigResponse(Response res)
        {
            // if the result is valid
            if (res.valid)
            {
                // set the running total equal to the answer
                runningTotal = Convert.ToDouble(res.ans);
                // add the answer to the text box
                textBox1.Text = res.ans;
                // set previous operation to = which passes the switch statement without doing anything
                previousOp = "=";
            }
            else // else if result invalid
            {
                // write error message to text box
                textBox1.Text = "An error occured communicating with the server";
                // print full error to the console for testing
                Console.WriteLine(res.ans);
                // set rpcError flag to true
                rpcError = true;
            }
            // set first flag for new number inputs
            firstFlag = true;
        }

        // number button handler
        private void numberBtn_Click(object sender, EventArgs e)
        {
            // pass the button text to numberInput function
            numberInput(((Button)sender).Text);
        }

        // function button handler
        private void functionBtn_Click(object sender, EventArgs e)
        {
            // pass the button text to the functionInput function
            functionInput(((Button)sender).Text);
        }

        // clear button handler
        private void clrBtn_Click(object sender, EventArgs e)
        {
            runningTotal = 0; // reset running total
            // display new running total
            textBox1.Text = Convert.ToString(runningTotal);
            previousOp = "+"; // reset previous op to plus
            firstFlag = true; // set first flag
            rpcError = false; // clear rpcError flag
        }

        // pi button handler
        private void piBtn_Click(object sender, EventArgs e)
        {
            // send pi from math lib to numberInput
            numberInput(Convert.ToString(Math.PI));
        }
        
        // decimal point button handler
        private void decPtBtn_Click(object sender, EventArgs e)
        {
            // if theres not already a decimal in the current input
            if (!textBox1.Text.Contains("."))
            {
                // send a decimal point to number input function
                numberInput(((Button)sender).Text);
            }
        }

        // angle unit button handler
        private void angUnitBtn_Click(object sender, EventArgs e)
        {
            // get ref to button
            Button self = (Button)sender;
            // if degree flag set
            if (deg)
            {
                // update angle button to indicate radians
                self.Text = "Mode: rad"
                deg = false; // clear degree flag
            }
            else // if degree flag unset
            {
                // update angle button to indicate degrees
                self.Text = "Mode: deg";
                deg = true; // set degree flag
            }
        }

        // Trig button handlers 
        // ====================

        // sine button handler
        private void sinBtn_Click(object sender, EventArgs e)
        {
            // call gRPC calculator DLL sine function, passing text box value and degree flag
            Response res = grpcCalc.Sine(val: Convert.ToDouble(textBox1.Text), deg: deg);

            // send response to handleTrigResponse function
            handleTrigResponse(res);
        }

        // cosine button handler
        private void cosBtn_Click(object sender, EventArgs e)
        {
            // call gRPC calculator DLL cosine function, passing textbox value and degree flag
            Response res = grpcCalc.Cos(val: Convert.ToDouble(textBox1.Text), deg: deg);
            
            // send response to handleTrigResponse function
            handleTrigResponse(res);
        }

        private void tanBtn_Click(object sender, EventArgs e)
        {
            // call gRPC calculator DLL tan function, passing textbox value and degree flag
            Response res = grpcCalc.Tan(val: Convert.ToDouble(textBox1.Text), deg: deg);
            
            // send response to handleTrigResponse function
            handleTrigResponse(res);
        }
    }
}

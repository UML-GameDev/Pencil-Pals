using System;
using System.Threading;
using TMPro;
using UnityEngine;

public class ConsoleController : MonoBehaviour
{
    private static bool hidden = true;

    // These are references to the TextMestPro input fields, usually you could assign them in the inspector by making them public but you cant assign static variable in the inspector.
    private static TMP_InputField inputField;
    private static TMP_InputField logField;
    private static Player player;
    public Animator animator;

    /*
     * This is a list of tuples (tuples are a way of packaging multiple variable into a single variable) that contain the command prefix as a string and an action (a pointer to a function without a return value)
     * to be taken if that prefix is entered. The action allows a string to be passed in as context, for example youy could have: "noclip on" or "noclip off".
     */
    private static Tuple<string, Action<string>>[] commandPrefixes = {
        Tuple.Create("runfast", new Action<string>(runfast)),
        Tuple.Create("give", new Action<string>(give)),
        Tuple.Create("rm", new Action<string>(remove)),
        Tuple.Create("swap", new Action<string>(swap))
    };

    /*
     * So far start is only used to initalize the two TextMeshPro input fields and temp character controller but this will be usefull to get other objects and scripts in the future. 
     */
    public void Start()
    {
        foreach (var gameObj in FindObjectsOfType(typeof(TMP_InputField)) as TMP_InputField[])
        {
            if (gameObj.name == "InputField")
            {
                inputField = gameObj;
            }
            else if (gameObj.name == "LogField")
            {
                logField = gameObj;
            }
        }
        player = GameObject.Find("Player").GetComponent<Player>();

        string startingText = "Welcome Back CodeBreaker\nAvailable commands: ";

        foreach (Tuple<string, Action<string>> command in commandPrefixes)
        {
            startingText += command.Item1 + " ";
        }

        logField.text = startingText + "\n";
    }

    /*
     * If console button has been pushed deploy the console.
     * Default console button is tab.
     */
    public void Update()
    {
        if (Input.GetButtonDown("Console"))
        {
            hidden = !hidden;
            animator.SetBool("Hidden", hidden);
            if (!hidden)
            {
                inputField.ActivateInputField();
            }
            else
            {
                inputField.DeactivateInputField();
            }
        }
        if (!hidden)
        {
            if (Input.GetButtonDown("Submit"))
            {
                submit(inputField.text);
            }
        }
    }

    /*
     * Function called when enter is pressed while console is deployed.
     */
    public void submit(string str)
    {
        logField.text += "CodeBreaker@" + str + "\n";
        detectCommand(str);
        inputField.text = "";
    }

    /*
     * This function detects command prefixes and send the context through to the appropriate function.
     */
    public void detectCommand(string str)
    {
        foreach (Tuple<string, Action<string>> tuple in commandPrefixes)
        {
            if (tuple.Item1.Equals(str))
            {
                tuple.Item2("");
                hidden = true;
                animator.SetBool("Hidden", hidden);
                return;
            }
            else if (str.StartsWith(tuple.Item1))
            {
                if (str[tuple.Item1.Length] == ' ')
                {
                    tuple.Item2(str.Substring(tuple.Item1.Length + 1));
                    hidden = true;
                    animator.SetBool("Hidden", hidden);
                    return;
                }
            }
        }
        logField.text += "Invalid command\n";
        inputField.ActivateInputField();
    }

    /*
     * Idea for implementation of setspeed
     */
    private static void runfast(string context)
    {
        float oldSpeed = 0;
        logField.text += "Runfast enabled for 10  sec.";
        //float oldspeed = player.getSpeed();
        //player.setSpeed(oldspeed*2);

        new Thread(()  =>
        {
            Thread.Sleep(20000);
            //player.setSpeed(oldspeed);
        }).Start();
    }

    /*
     * Gives player items.
     */
    private static void give(string context)
    {
        //if context is a recognized item
        //player.give(context)
    }
    /*
     * Removes walls from vicinity of player.
     */
    private static void remove(string context)
    {
        // get player location
        // determine if there are any glitched walls in the vicinity of player
        // remove the glitched walls
    }

    /*
     * flips flip flop/switching walls
     */
    private static void swap(string context)
    {
        // swap all switching walls
    }
}

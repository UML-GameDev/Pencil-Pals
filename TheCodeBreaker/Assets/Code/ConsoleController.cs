using System;
using TMPro;
using UnityEngine;

public class ConsoleController : MonoBehaviour
{
    private static Boolean hidden = true;

    // These are references to the TextMestPro input fields, usually you could assign them in the inspector by making them public but you cant assign static variable in the inspector.
    private static TMP_InputField inputField;
    private static TMP_InputField logField;
    private static Player player;
    public Animator animator;

    /*
     * This is a list of tuples (tuples are a way of packaging multiple variable into a single variable) that contain the command prefix as a string and an action (a pointer to a function without a return value)
     * to be taken if that prefix is entered. The action allows a string to be passed in as context, for example youy could have: "noclip on" or "noclip off".
     */
    private Tuple<string, Action<string>>[] commandPrefixes = {
        Tuple.Create("noclip", new Action<string>(noclip))
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
        }
    }

    /*
     * This function is called using the textmeshpro Text Area's 'On End Edit' function and passes a dynamic string to the function.
     * This is the easiest way of detecting the user hitting the enter button however, it is also called if the user deselects or clicks off of the UI element so it wont be so helpfull long term.
     */
    public void done_edit(string str)
    {
        logField.text += "Username@tablet>>" + str + "\n";
        detectCommand(str);
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
                return;
            }
            else if(str.StartsWith(tuple.Item1))
            {
                if(str[tuple.Item1.Length] == ' ')
                {
                    tuple.Item2(str.Substring(tuple.Item1.Length+1));
                    return;
                }
            }
        }
        logField.text += "Not a valid command\n";
    }

    /*
     * This is a test/Example function to act as one of the actions for a command prefix.
     * Note: all actions must be static
     */
    private static void noclip(string context)
    {
        logField.text += "Do the Damn thing!\n";
    }
}

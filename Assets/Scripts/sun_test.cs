using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class sun_test : MonoBehaviour {

    static int[] colourArray;
    static int n_update;
    float[] redArray;
    float[] greenArray;
    float[] blueArray;
    int a_size;

    void makeColourArray()
    {
        int steps = 200;
        a_size = steps * 6;


        redArray = new float[a_size];
        greenArray = new float[a_size];
        blueArray = new float[a_size];

        int so_far = 0;

        float c_r = 255;
        float c_g = 255;
        float c_b = 255;

        float step_interval;


        for (int i = 0; i < steps; i++) {
            //white-blue
            step_interval = (float)255 / steps;
            redArray[i + so_far] = c_r - i*step_interval; // +-i
            greenArray[i + so_far] = c_g;
            blueArray[i + so_far] = c_b;
        }

        c_r = redArray[so_far + steps - 1];
        c_g = greenArray[so_far + steps - 1];
        c_b = blueArray[so_far + steps - 1];

        so_far += steps;
        //

        for (int i = 0; i < steps; i++)
        {
            //blue-yellow
            step_interval = (float)255 / steps;
            redArray[i + so_far] = c_r + i * step_interval; // +-i
            greenArray[i + so_far] = c_g;
            blueArray[i + so_far] = c_b  - i * step_interval;
        }

        c_r = redArray[so_far + steps - 1];
        c_g = greenArray[so_far + steps - 1];
        c_b = blueArray[so_far + steps - 1];

        so_far += steps;

        for (int i = 0; i < steps; i++)
        {
            //yellow-black
            step_interval = (float)255 / steps;
            redArray[i + so_far] = c_r - (i * step_interval)/3; // +-i
            greenArray[i + so_far] = c_g - (i * step_interval)/3;
            blueArray[i + so_far] = c_b; //- i * step_interval;
        }

        c_r = redArray[so_far + steps - 1];
        c_g = greenArray[so_far + steps - 1];
        c_b = blueArray[so_far + steps - 1];

        so_far += steps;


        for (int i = 0; i < steps; i++)
        {
            //black-blue
            step_interval = (float)255 / steps;
            redArray[i + so_far] = c_r;// - (i * step_interval) / 3; // +-i
            greenArray[i + so_far] = c_g;// - (i * step_interval) / 3;
            blueArray[i + so_far] = c_b + i * step_interval;
        }

        c_r = redArray[so_far + steps - 1];
        c_g = greenArray[so_far + steps - 1];
        c_b = blueArray[so_far + steps - 1];

        so_far += steps;


        for (int i = 0; i < steps; i++)
        {
            //blue-violet
            step_interval = (float)255 / steps;
            redArray[i + so_far] = c_r + (i * step_interval); // +-i
            greenArray[i + so_far] = c_g;// - (i * step_interval) / 3;
            blueArray[i + so_far] = c_b;// + i * step_interval;
        }

        c_r = redArray[so_far + steps - 1];
        c_g = greenArray[so_far + steps - 1];
        c_b = blueArray[so_far + steps - 1];

        so_far += steps;

        for (int i = 0; i < steps; i++)
        {
            //violet - pink - white
            step_interval = (float)255 / steps;
            redArray[i + so_far] = c_r;// + (i * step_interval); // +-i
            greenArray[i + so_far] = c_g+ (i * step_interval) / 3;
            blueArray[i + so_far] = c_b;// + i * step_interval;
        }

        c_r = redArray[so_far + steps - 1];
        c_g = greenArray[so_far + steps - 1];
        c_b = blueArray[so_far + steps - 1];

        so_far += steps;
    }
	// Use this for initialization
	void Start () {
        n_update = 0;
        makeColourArray();

    }

    // Update is called once per frame
    void Update() {
        {
            Sprite spr = Resources.Load<Sprite>("bg_sky");
            SpriteRenderer sprRenderer = (SpriteRenderer)GetComponent<Renderer>();

            Color c_col = new Color((float)redArray[n_update] / 255, (float)greenArray[n_update] / 255, (float)blueArray[n_update] / 255, (float)0.7);
            sprRenderer.color = c_col;

            if (n_update < a_size-1)
            {
                 n_update += 1;
            }
            else
            {
                n_update = 0;
            }
            
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stack : MonoBehaviour
{
    public int top = -1;
    public int lives = 3;
    public Slider slider;
    public GameObject[] balls;
    public GameObject instance, dailNumbers, explosion, form;

    private Image controls;
    // Start is called before the first frame update
    void Start()
    {
        dailNumbers.SetActive(false);
        controls = GameObject.FindGameObjectWithTag("controls").GetComponent<Image>();
        controls.enabled = false;
        StartCoroutine(stackGenerate(balls.Length - 1));
    }

    void push(char ch)
    {
        balls[++top] = Instantiate(instance);
        balls[top].GetComponentInChildren<Text>().text = ch.ToString();
    }

    char pop()
    {
        if (top > -1)
        {
            GameObject temp = balls[top];
            explode(balls[top]);
            Destroy(balls[top--]);
            return temp.GetComponentInChildren<Text>().text[0];
        }
        else
            return '$';
    }

    char peak()
    {
        return balls[top].GetComponentInChildren<Text>().text[0];
    }

    public string postfix(string infix)
    {
        // infix = "3+4-(7+3*2)"
        char[] arr = infix.ToCharArray();
        string text = "";
        for (int i = 0; i < infix.Length; i++)
        {
            if(arr[i] >= '0' && arr[i] <= '9')
                text += arr[i]; // output
            else
            {
                //putting in the stack
                if (arr[i] == '(')
                    push(arr[i]);
                else if(arr[i] == ')')
                {
                    while (peak() != '(')
                        text += pop();
                    pop();
                }
                else
                {
                    while (peak() != '$' && predicates(arr[i]) <= predicates(peak()))
                        text += pop();
                    push(arr[i]);
                }
            }
        }
        while (peak() != '$')
            text += pop();
        return text;
    }

    private int predicates(char ch)
    {
        if (ch == '+' || ch == '-')
            return 1;
        if (ch == '*' || ch == '/')
            return 2;
        return 0;

    }


    private IEnumerator stackGenerate(int count)
    {
        yield return new WaitForSeconds(1);
        if (count >= 0)
        {
            int random = Random.Range('1', '9');
            push((char)random);
            StartCoroutine(stackGenerate(count - 1));
        }
        yield return new WaitForSeconds((float)count + 1);
        dailNumbers.SetActive(true);
    }

    public void Dail(int num)
    {
        num += 48;
        char ch = (char)num;
        Debug.Log(ch);

        if (ch == peak())
            pop();
        else
        {
            lives--;
            slider.value = lives;
            if(lives <= 0)
            {
                dailNumbers.SetActive(false);
                StartCoroutine(cleared(false));
            }
                
        }
        if (top == -1)
            StartCoroutine(cleared(true));
    }

    private IEnumerator cleared(bool flag)
    {
        yield return new WaitForSeconds(2);
        SecurityClear.isClear = flag;
        form.SetActive(true);
        Destroy(gameObject);
    }

    private void explode(GameObject ball)
    {
        GameObject obj = Instantiate(explosion, ball.transform.position, ball.transform.rotation);
        Destroy(obj, 3);
    }
    private void OnDestroy()
    {
        controls.enabled = true;
    }
}

using UnityEngine;
using System.Collections;
public class animations : MonoBehaviour
{
    public Animator anim_;
    public Animator pauseTextAnim; 
   
    
   public void fadeInAnim()
    {
        anim_.SetBool("fading",true);
    }
    public void fadeOutAnim()
    {
        anim_.SetBool("fading",false);
    }

    public void bounceText()
    {
        pauseTextAnim.SetBool("moving", true);
    }
}

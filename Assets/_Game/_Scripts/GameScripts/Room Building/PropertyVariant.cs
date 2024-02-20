using UnityEngine;

public class PropertyVariant : MonoBehaviour
{
    public Sprite variantImage;
    public Animator propertyAnimator;
    public void Activate(bool firstTime = false)
    {
        if (propertyAnimator == null)
        {
            propertyAnimator = GetComponent<Animator>();
        }


        gameObject.SetActive(true);

        if (propertyAnimator != null)
        {
            switch (firstTime)
            {
                case true:
                    propertyAnimator.SetTrigger("Enable");
                    break;
                default:
                    propertyAnimator.SetTrigger("Activate");
                    break;
            }
        }
    }
    public void Deactivate() 
    {
        gameObject.SetActive(false);
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Paralax : MonoBehaviour
{
    //public Sprite background;
    public List<Transform> target;
    public float speed = 1;
    public float tick = 0;
    private IParalax[] transforms;
    private RectTransform transform;

    // Start is called before the first frame update
    void Start()
    {
        transform = (RectTransform)gameObject.transform;
        transforms = new IParalax[target.Count];
        for(int i = 0; i < transforms.Length; i++){
            if(target[i].GetComponent<Image>() != null){
                transforms[i] = new ImageParalax(target[i].GetComponent<Image>(), 3);
            } else {
                throw new Exception("cannot use not image paralax (" + i + ")");
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < transforms.Length; i++){
            transforms[i].Move(tick * speed, transform.rect.width);
        }
    }

    interface IParalax{
        void Move(float step, float parentWidth);
    }

    class ImageParalax : IParalax
    {
        private Image[] images;
        private float allWidth;
        public ImageParalax(Image image, int duplicate){
            images = new Image[Math.Max(1, duplicate)];
            images[0] = image;
            RectTransform rt = (RectTransform)image.transform;
            Vector2 lp = rt.anchoredPosition;
            float imgWidth = rt.rect.width; //image.GetComponent<SpriteRenderer>().bounds.size.x;
            for(int i = 1; i < duplicate; i++){
                Image img = UnityEngine.Object.Instantiate(image);
                img.transform.SetParent(rt.parent);
                img.transform.position = rt.position;
                img.transform.localScale = rt.localScale;
                ((RectTransform)img.transform).anchoredPosition = new Vector2(lp.x + imgWidth * i, lp.y);
                images[i] = img;
            }
            allWidth = imgWidth * images.Length;
            Debug.Log("all w = " + allWidth);
        }
        public void Move(float step, float parentWidth)
        {
            for(int i = 0; i < images.Length; i++){
                RectTransform rt = (RectTransform)images[i].transform;
                Vector2 old = rt.anchoredPosition;
                float imgWidth = rt.rect.width; //images[i].GetComponent<SpriteRenderer>().bounds.size.x;
                float x = old.x - step;
                if(step > 0 && x + imgWidth <= 0){
                    Debug.Log("x (" + x + ") + img w (" + imgWidth + ") < 0 -> x + all w");
                    x += allWidth;
                } else if(step < 0 && x > parentWidth){
                    Debug.Log("x (" + x + ") > par w (" + parentWidth + ") -> x - all w");
                    x -= allWidth;
                }
                rt.anchoredPosition = new Vector2(x, old.y);
            }
        }
    }
}

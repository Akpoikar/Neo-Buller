using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Elementals { Standard, Fire, Water, Electro, Cryo, Crystal }

public class Elements : MonoBehaviour
{
    public Elementals element;

    // Component
    private Renderer _renderRightGun;
    private Renderer _renderLeftGun;
    private Renderer _renderPlayer;
    private Renderer _renderBullet;

    // Game Object
    public GameObject objRightGun;
    public GameObject objLeftGun;
    public GameObject objPlayer;
    public GameObject objBullet;


    public Material colorCryo;
    public Material colorFire  ;
    public Material colorWater ;
    public Material colorElectro ;
    public Material colorStandard ;
    public Material colorCrystal ;
    
    public Color getcolorCryo;
    public Color getcolorFire  ;
    public Color getcolorWater ;
    public Color getcolorElectro ;
    public Color getcolorStandard ;
    public Color getcolorCrystal ;


    // Start is called before the first frame update
    void Start()
    {
        _renderRightGun = objRightGun.GetComponent<Renderer>();
        _renderLeftGun = objLeftGun.GetComponent<Renderer>();
        _renderPlayer = objPlayer.GetComponent<Renderer>();

        element = Elementals.Standard;
       // _renderBullet = objBullet.GetComponent<Renderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("1"))
        {
            ElementChange(Elementals.Cryo);
            element = Elementals.Cryo;
        }

        if (Input.GetKeyDown("2"))
        {
            ElementChange(Elementals.Fire);
            element = Elementals.Fire;
        }

        if (Input.GetKeyDown("3"))
        {
            ElementChange(Elementals.Water);
            element = Elementals.Water;
        }

        if (Input.GetKeyDown("4"))
        {
            ElementChange(Elementals.Electro);
            element = Elementals.Electro;
        }
        /*else
        {
            ElementChange(Elementals.Standard);
        }*/
    }
    
    public Color GetColorOnElement()
    {
        return element switch
        {
            Elementals.Fire=>getcolorFire,
            Elementals.Electro=>getcolorElectro,
            Elementals.Cryo=>getcolorCryo,
            Elementals.Water=>getcolorWater,
            _ =>getcolorStandard
        };
    } 
    
    public Material GetMaterialOnElement()
    {
        return element switch
        {
            Elementals.Fire=>colorFire,
            Elementals.Electro=>colorElectro,
            Elementals.Cryo=>colorCryo,
            Elementals.Water=>colorWater,
            _ => colorStandard
        };
    }

    void ElementChange(Elementals state)
    {
        if (state == Elementals.Cryo)
        {
            var mats = new Material[] { _renderRightGun.materials[0], colorCryo };
            _renderRightGun.sharedMaterials = mats;
            
            var mats1 = new Material[] { _renderLeftGun.materials[0], colorCryo };
            _renderLeftGun.sharedMaterials = mats1;

            _renderPlayer.material = colorCryo;
     //       _renderBullet.material.SetColor("_Color", colorCryo);
        }

        if (state == Elementals.Fire)
        {
            var mats = new Material[] { _renderRightGun.materials[0], colorFire };
            _renderRightGun.sharedMaterials = mats;  
            
            var mats1 = new Material[] { _renderLeftGun.materials[0], colorFire };
            _renderLeftGun.sharedMaterials = mats1;

            _renderPlayer.material = colorFire;
     //       _renderBullet.material.SetColor("_Color", colorFire);
        }

        if (state == Elementals.Water)
        {
            var mats = new Material[] { _renderRightGun.materials[0], colorWater };
            _renderRightGun.sharedMaterials = mats;

            var mats1 = new Material[] { _renderLeftGun.materials[0], colorWater };
            _renderLeftGun.sharedMaterials = mats1;

            _renderPlayer.material = colorWater;
            //       _renderBullet.material.SetColor("_Color", colorStandard);
        }

        if (state == Elementals.Standard)
        {
            var mats = new Material[] { _renderRightGun.materials[0], colorStandard };
            _renderRightGun.sharedMaterials = mats;
            
            var mats1 = new Material[] { _renderLeftGun.materials[0], colorStandard };
            _renderLeftGun.sharedMaterials = mats1;
            
            _renderPlayer.material = colorStandard;
     //       _renderBullet.material.SetColor("_Color", colorStandard);
        }
        if (state == Elementals.Electro)
        {
            var mats = new Material[] { _renderRightGun.materials[0], colorElectro };
            _renderRightGun.sharedMaterials = mats; 
            
            var mats1 = new Material[] { _renderLeftGun.materials[0], colorElectro };
            _renderLeftGun.sharedMaterials = mats1;
            
            _renderPlayer.material = colorElectro;
       //     _renderBullet.material.SetColor("_Color", colorElectro);
        }
    }
}
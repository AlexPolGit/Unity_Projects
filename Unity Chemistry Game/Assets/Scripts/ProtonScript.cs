using UnityEngine;
using System;
using System.Collections.Generic;

public class ProtonScript : MonoBehaviour
{
    public float bobbingWeakness = 50.0f, bobbingSlowness = 100.0f;

    private int happyNumber = -2, chargeZeroNumber;
    private bool isHappy = false;
    private Color ballColor;
    private SoundScript sound;
    private float bobbing = 0.0f;
    private int orbitingElectrons = 0, elementNumber;
    private Camera cam;
    private Texture happy, sad;
    private Dictionary<GameObject, ElectronScript> ownedElectrons;
    public Elements element;
    public ElementTypes elementType;
    public BondingTypes bonds;

    public enum BondingTypes
    {
        NONE, IONIC, COVALENT, BOTH
    }

    public enum ElementTypes
    {
        NOTYPE, METAL, NONMETAL, METALLOID
    }

    public enum Elements
    {
        NOELEMENT,
        H, He,
        Li, Be, B, C, N, O, F, Ne, Na, Mg, Al, Si, P, S, Cl, Ar,
        K, Ca, Sc, Ti, V, Cr, Mn, Fe, Co, Ni, Cu, Zn, Ga, Ge, As, Se, Br, Kr,
        Rb, Sr, Y, Zr, Nb, Mo, Tc, Ru, Rh, Pd, Ag, Cd, In, Sn, Sb, Te, I, Xe,
        Cs, Ba, La, Ce, Pr, Nd, Pm, Sm, Eu, Gd, Tb, Dy, Ho, Er, Tm, Yb, Lu, Hf, Ta, W, Re, Os, Ir, Pt, Au, Hg, Tl, Pb, Bi, Po, At, Rn,
        Fr, Ra, Ac, Th, Pa, U, Np, Pu, Am, Cm, Bk, Cf, Es, Fm, Md, No, Lr, Rf, Db, Sg, Bh, Hs, Mt, Ds, Rg, Cn, Nh, Fl, Mc, Lv, Ts, Og
    };

    /// <summary>
    /// Sets up the proton (when called) for use in the scene.
    /// </summary>
    /// <param name="n"></param>

    public void setUpProton(int n)
    {
        cam = Camera.main;
        sound = GameObject.Find("Sound Player").GetComponent<SoundScript>();
        happy = Resources.Load("Textures/happyFace") as Texture2D;
        sad = Resources.Load("Textures/sadFace") as Texture2D;

        System.Random r = new System.Random();

        if (n <= 7)
        {
            ballColor = new Color(1.0f, 0.0f, 0.0f + (0.14f * (n - 1)), 0.0f);
        }
        else if (n <= 14)
        {
            ballColor = new Color(1.0f - (0.14f * ((n - 1) % 7)), 0.0f, 1.0f, 0.0f);
        }
        else if (n <= 20)
        {
            ballColor = new Color(0.0f, 0.0f + (0.14f * ((n % 7) - 1)), 1.0f, 0.0f);
        }
        else
        {
            ballColor = new Color((float)((r.NextDouble() * 1.0) - 0.0), (float)((r.NextDouble() * 1.0) - 0.0), (float)((r.NextDouble() * 1.0) - 0.0), 0.0f);
        }

        GetComponent<MeshRenderer>().material.color = ballColor;

        if (n <= 2)
        {
            chargeZeroNumber = n;
        }
        else if (n <= 10)
        {
            chargeZeroNumber = n - 2;
        }
        else if (n <= 18)
        {
            chargeZeroNumber = n - 10;
        }
        else if (n <= 20)
        {
            chargeZeroNumber = n - 18;
        }

        switch (n)
        {
            case 1:
                {
                    happyNumber = -1;
                    bonds = BondingTypes.BOTH;
                    elementType = ElementTypes.NONMETAL;
                    break;
                }
            case 2:
                {
                    happyNumber = 2;
                    bonds = BondingTypes.COVALENT;
                    elementType = ElementTypes.NONMETAL;
                    break;
                }
            case 3:
                {
                    happyNumber = 0;
                    bonds = BondingTypes.IONIC;
                    elementType = ElementTypes.METAL;
                    break;
                }
            case 4:
                {
                    happyNumber = 0;
                    bonds = BondingTypes.IONIC;
                    elementType = ElementTypes.METAL;
                    break;
                }
            case 5:
                {
                    happyNumber = 0;
                    bonds = BondingTypes.IONIC;
                    elementType = ElementTypes.METALLOID;
                    break;
                }
            case 6:
                {
                    happyNumber = -1;
                    bonds = BondingTypes.COVALENT;
                    elementType = ElementTypes.NONMETAL;
                    break;
                }
            case 7:
                {
                    happyNumber = 8;
                    bonds = BondingTypes.BOTH;
                    elementType = ElementTypes.NONMETAL;
                    break;
                }
            case 8:
                {
                    happyNumber = 8;
                    bonds = BondingTypes.BOTH;
                    elementType = ElementTypes.NONMETAL;
                    break;
                }
            case 9:
                {
                    happyNumber = 8;
                    bonds = BondingTypes.BOTH;
                    elementType = ElementTypes.NONMETAL;
                    break;
                }
            case 10:
                {
                    happyNumber = 8;
                    bonds = BondingTypes.BOTH;
                    elementType = ElementTypes.NONMETAL;
                    break;
                }
            case 11:
                {
                    happyNumber = 0;
                    bonds = BondingTypes.IONIC;
                    elementType = ElementTypes.METAL;
                    break;
                }
            case 12:
                {
                    happyNumber = 0;
                    bonds = BondingTypes.IONIC;
                    elementType = ElementTypes.METAL;
                    break;
                }
            case 13:
                {
                    happyNumber = 0;
                    bonds = BondingTypes.IONIC;
                    elementType = ElementTypes.METAL;
                    break;
                }
            case 14:
                {
                    happyNumber = -1;
                    bonds = BondingTypes.COVALENT;
                    elementType = ElementTypes.METALLOID;
                    break;
                }
            case 15:
                {
                    happyNumber = 8;
                    bonds = BondingTypes.BOTH;
                    elementType = ElementTypes.NONMETAL;
                    break;
                }
            case 16:
                {
                    happyNumber = 8;
                    bonds = BondingTypes.BOTH;
                    elementType = ElementTypes.NONMETAL;
                    break;
                }
            case 17:
                {
                    happyNumber = 8;
                    bonds = BondingTypes.BOTH;
                    elementType = ElementTypes.NONMETAL;
                    break;
                }
            case 18:
                {
                    happyNumber = 8;
                    bonds = BondingTypes.BOTH;
                    elementType = ElementTypes.NONMETAL;
                    break;
                }
            case 19:
                {
                    happyNumber = 0;
                    bonds = BondingTypes.IONIC;
                    elementType = ElementTypes.METAL;
                    break;
                }
            case 20:
                {
                    happyNumber = 0;
                    bonds = BondingTypes.IONIC;
                    elementType = ElementTypes.METAL;
                    break;
                }
            default:
                {
                    happyNumber = -2;
                    bonds = BondingTypes.NONE;
                    elementType = ElementTypes.NOTYPE;
                    break;
                }
        }

        try
        {
            element = (Elements)n;
            elementNumber = n;
            //Debug.Log(name + " IS NUMBER " + elementNumber + ", TYPE " + element);
        }
        catch (NullReferenceException e)
        {
            Debug.Log(name + " HAS INVALID NAME. " + e.ToString());
        }

        ownedElectrons = new Dictionary<GameObject, ElectronScript>();
    }

    /// <summary>
    /// Resets the orbits of the electrons around this atom; used when an electron is gained or lost.
    /// </summary>

    public void resetOrbits()
    {
        foreach (ElectronScript es in ownedElectrons.Values)
        {
            es.resetTheta();
            es.gameObject.name = "E" + (es.getElectronNumber()) + " of [" + es.getParentProton().name + "]";
        }
    }

    /// <summary>
    /// Updates the proton every frame (when called).
    /// </summary>

    public void updateProton()
    {
        orbitingElectrons = ownedElectrons.Count;
        updateElectronNumbers();

        foreach (GameObject e in ownedElectrons.Keys)
        {
            ownedElectrons[e].doOrbit();
        }

        transform.Translate(new Vector3(0, Mathf.Sin(bobbing) / bobbingWeakness, 0));

        if (bobbing >= (Mathf.PI * 2))
        {
            bobbing = 0;
        }
        else
        {
            bobbing += (Mathf.PI * 2) / bobbingSlowness;
        }

        if (happyNumber == orbitingElectrons)
        {
            isHappy = true;
            GetComponent<MeshRenderer>().material.mainTexture = happy;
        }
        else
        {
            isHappy = false;
            GetComponent<MeshRenderer>().material.mainTexture = sad;
        }
    }

    /// <summary>
    /// Updates special values on this atom's electrons so that orbits can work.
    /// </summary>

    private void updateElectronNumbers()
    {
        byte n = 0;

        foreach (GameObject e in ownedElectrons.Keys)
        {
            ownedElectrons[e].setElectronNumber(n);
            ownedElectrons[e].setTotalElectrons(orbitingElectrons);
            n++;
        }
    }

    /// <summary>
    /// Add an electron this atom's orbit.
    /// </summary>
    /// <param name="electron"></param>

    public void addElectron(GameObject electron)
    {
        ownedElectrons.Add(electron, electron.GetComponent<ElectronScript>());
        orbitingElectrons++;
        updateElectronNumbers();
    }

    /// <summary>
    /// Remove an electron from this atom's orbit.
    /// </summary>
    /// <param name="electron"></param>

    public void removeElectron(GameObject electron)
    {
        ownedElectrons.Remove(electron);
        orbitingElectrons--;
        updateElectronNumbers();
    }

    public void setElementBySymbol(string symbol)
    {
        element = (Elements)Enum.Parse(typeof(Elements), symbol);
    }

    public void setElementByNumber(int num)
    {
        setElementBySymbol(num.ToString());
    }

    /// <summary>
    /// Is this atom happy? Compares valence electrons to currently orbiting electrons.
    /// </summary>

    public void checkIfHappy()
    {
        if (orbitingElectrons == happyNumber)
        {
            isHappy = true;
        }
        else
        {
            isHappy = false;
        }
    }

    public Color getBallColor()
    {
        return ballColor;
    }

    public bool isAtomHappy()
    {
        return isHappy;
    }

    public int getHappyNumber()
    {
        return happyNumber;
    }

    public int getZeroChargeNumber()
    {
        return chargeZeroNumber;
    }
}
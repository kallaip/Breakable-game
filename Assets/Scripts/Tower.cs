using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{

    private Platform[,] platforms = null;
    public int stageHeight;
    public int stages;
    public float scaleFactor = 1f;
    public Platform[] platformPrefabs;
	public Konzol konzolPrefab;
    private Vector3 origin;
	private Konzol console;
    public Player player;
    public bool setPlayerPosition = false;
    public int stagesBetweenWalls = 5;
   

    private int[] vwalls = new int[4];
    private int[] hwalls = new int[20];
    private int max_h_walls;

    //slot: megmutatja, hanyadik helyen kezdődjön a platform
    // x tengely: mennyit rotálunk
    // y tengely: hanyadik emelet

    public Vector2 addPlatform(Platform platform, Vector2 slot)
    {

        if (slot.x + platform.occupiedPlace <= 24)
        {

            Vector3 position = origin + new Vector3(0, slot.y * stageHeight, 0);
            float rotation = slot.x * 15;

            platforms[(int)slot.x, (int)slot.y] = Instantiate(platform, position, Quaternion.identity);

            platforms[(int)slot.x, (int)slot.y].transform.Rotate(new Vector3(0, rotation + 90, 0));
            Vector3 myscale = platforms[(int)slot.x, (int)slot.y].transform.localScale;
            myscale.x *= scaleFactor;
            myscale.y *= scaleFactor;
            platforms[(int)slot.x, (int)slot.y].transform.localScale = myscale;


            Vector3 consolePos = position - new Vector3(0, 0.3f, 0);
            // első tartó
            console = Instantiate(konzolPrefab, consolePos, Quaternion.identity);
            console.transform.Rotate(new Vector3(0, rotation + 1f, 0));
            Vector3 myscale1 = console.transform.localScale;
            myscale1.z *= scaleFactor;
            console.transform.localScale = myscale1;
            // középső tartó
            console = Instantiate(konzolPrefab, consolePos, Quaternion.identity);
			console.transform.Rotate(new Vector3(0, rotation + (platform.occupiedPlace*15)/2+1.1f, 0));
            Vector3 myscale2 = console.transform.localScale;
            myscale2.z *= scaleFactor;
            console.transform.localScale = myscale2;
            // másik vége
            console = Instantiate(konzolPrefab, consolePos, Quaternion.identity);
            console.transform.Rotate(new Vector3(0, rotation + (platform.occupiedPlace * 15) - 1f, 0));
            Vector3 myscale3 = console.transform.localScale;
            myscale3.z *= scaleFactor;
            console.transform.localScale = myscale3;

            if (slot.x + platform.occupiedPlace == 24)
            {
                slot.x = 0;
                slot.y++;
            } else
            {
                slot.x += platform.occupiedPlace;
            }

        }
        return slot;
    }

    public void removePlatform(Vector2 slot)
    {
        if (platforms[(int)slot.x, (int)slot.y] != null)
        {
            Destroy(platforms[(int)slot.x, (int)slot.y].gameObject);
            platforms[(int)slot.x, (int)slot.y] = null;
        }
    }

    private void populate()
    {
        

        Vector2 newSlot = new Vector2(0, 0);
        int stage = 0;
        shit_in();

        //minden függőleges hézagba 1db random átjárót beteszünk
        for (int i = 0; i < 4; i++)
        {
            Vector2 oneSlot = new Vector2( vwalls[i], (int)Random.Range(i*(stages/4), (i+1)*(stages / 4)));
            oneSlot = addPlatform(platformPrefabs[0], oneSlot);
        }
        for(int i=0; stage < stages; i++)
        {
            // vízszintes falak kihagyása
            for (int j = 0; j < max_h_walls; j++)
            {
                if(hwalls[j] == newSlot.y)
                {
                    newSlot.y = newSlot.y + 1;
                    stage = (int)newSlot.y;
                }
            }


            //random platformtípus (60-ast nem használjuk)
            int platformType = (int)Random.Range(1, 4);
            // megnézzük, hogy falba ütközünk-e?
            //a platformtype egyenlő az elfoglalt hellyel
            for(int j=0; j<=3; j++)
            {
                if( (vwalls[j] >= newSlot.x) && (vwalls[j] <=newSlot.x + platformType))
                {
                    if(vwalls[j]==newSlot.x)
                    {
                        newSlot.x = newSlot.x + 1;
                        if (newSlot.x == 24)
                        {
                            newSlot.y = newSlot.y + 1;
                            newSlot.x = 0;
                        }
                    } else
                    {
                        platformType = vwalls[j] - (int)newSlot.x;
                        
                    }
                } 
            }
            newSlot = addPlatform(platformPrefabs[platformType - 1], newSlot);
            stage = (int)newSlot.y;

        }



        if (setPlayerPosition && player)
        {
            // egyik függőleges hézag melletti felső platformra tesszük a játékost

            
            Vector3 playerPosition = origin + new Vector3(0, newSlot.y * stageHeight + 0.2f, 0) + Vector3.right * 48 * scaleFactor + Vector3.left*3;

            float rotation = newSlot.x * 15 + 7;
            playerPosition = Quaternion.Euler(0, rotation, 0) * playerPosition;
            player.setPosition(playerPosition);
            
            
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (stages > 1000) stages = 1000;
        if (stages < 1) stages = 1;
        platforms = new Platform[24, stages];

        origin = transform.position;
        populate();
    }


    private void shit_in()
    {
        max_h_walls = (int) Mathf.Floor(stages / stagesBetweenWalls);
        if (max_h_walls > 20) max_h_walls = 20;

         //kijelölünk 4 db függőleges "falat", eltároljuk hol vannak
        // legalább 2 platformnak lenni kell a falak között (ami valójában lyuk)
               
        vwalls[0] = (int)Random.Range(1, 6);
        vwalls[1] = (int)Random.Range(9, 11);
        vwalls[2] = (int)Random.Range(14, 16);
        vwalls[3] = (int)Random.Range(19, 22);

        //ugyanaz vízszintes falakra
        for(int i=0; i < max_h_walls; i++)
        {
            hwalls[i] = (int)Random.Range(i* stagesBetweenWalls, (i+1)* stagesBetweenWalls);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

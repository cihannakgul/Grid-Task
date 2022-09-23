using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int nWidth;
    [SerializeField] private int nHeight;
    [SerializeField] private Node nodePrefab;
    [SerializeField] private SpriteRenderer boardPrefab;
    int spawningCounts;
    

    List<Node> nodes = new List<Node>();
    void Start()
    {
        GenerateGrid();   
    }

    void GenerateGrid()
    {
        for (int i = 0; i < nWidth; i++)
        {
            for (int y = 0; y < nHeight; y++)
            {
                Node node = Instantiate(nodePrefab, new Vector2(i,y), Quaternion.identity);
                 node.name = i + "Node" + y;
                nodes.Add(node);
            }
        }
        Vector2 center = new Vector2((float)nWidth / 2 - 0.5f, (float)nHeight / 2 - 0.5f);
        var board = Instantiate(boardPrefab, center, Quaternion.identity);
        board.size = new Vector2(nWidth, nHeight);
        CamPositionAdapter(center);
    }

    void CamPositionAdapter(Vector2 center)
    {

        Camera.main.transform.position = new Vector3(center.x, center.y, -10f);
        int camSize = (nHeight / 2) + 1;
        Camera.main.orthographicSize = camSize;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        { // if left button pressed...
            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit, 1000.0f))
            {
                spawningCounts++;
                hit.transform.tag = "Opened";
                hit.transform.GetChild(1).gameObject.SetActive(true);
              //  CheckVerticals();
                //if (spawningCounts>2)
                //{
                  
                   
                //    CheckComplete(returnID(hit.transform));
                //    spawningCounts = 0;
                //}
             

            }
        }
    }
    void CheckVerticals()
    {

        List<Node> listOf = new List<Node>();
        int last = nWidth - 1;
        int temp;
        
        for (int i = last; i < 0; i--)
        {
           
            if (nodes[i].tag == nodes[i-1].tag)
            {
                if (nodes[i-1].tag == nodes[i-2].tag)
                {
                     Destroy(nodes[i]);
                    Destroy(nodes[i-1]);
                    Destroy(nodes[i-2]);
                }
            }
        }

    }

    void CheckComplete(int hitID) // This function only works for square
    {
        int last = nWidth-1;
        // for left
        CheckTo(nWidth, hitID);
     
        //Checking(-nWidth, hitID);
        //Checking(1, hitID);
        //Checking(-1, hitID);

        
        
    }
    void CheckTo(int downsize, int mine)
    {
        if (mine - downsize > 0)
        {

            string tagOne = nodes[mine - downsize].gameObject.tag;
            if((mine-(2*downsize))>0)
            {
                string tagTwo = nodes[mine - (2 * downsize)].gameObject.tag;
                if (tagOne == "Opened" && tagTwo == "Opened")
                {
                    nodes[mine - downsize].gameObject.SetActive(false);
                    nodes[mine - (2 * downsize)].gameObject.SetActive(false);
                    //   nodes[mine].gameObject.SetActive(false);
                }

            }

           
        }
       
    }
    void Checking(int downsize, int mine)
    {
        List<Node> neighborNodes = new List<Node>();
        if (mine-downsize>0)
        {
            
            for (int i = 1; i < 3; i++)
            {
                if (mine-(i*downsize)>=0)
                {
                    if (nodes[mine - (i * downsize)].gameObject.tag == "Opened")
                    {
                        neighborNodes.Add(nodes[mine - (i * downsize)]);
                        if (i == 2)
                        {
                            neighborNodes[0].gameObject.SetActive(false);
                            neighborNodes[1].gameObject.SetActive(false);
                            neighborNodes.Clear();
                          
                        }
                    }
                }
               
               
            }
        }
      
        

    }

    int returnID(Transform hitter)
    {
        for (int i = 0; i < nodes.Count; i++)
            if (nodes[i].name == hitter.gameObject.name)
                return i;

        return 0;


    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;



//keep track of all of the map positions/neighbours
//will create our path, but we'll know what is going on there
public class PathMarker{
    public MapLocation location;
    public float G;
    public float H;
    public float F;
    public GameObject marker; //phisical marker itself
    public PathMarker parent; //marker ->
    

    public PathMarker(MapLocation l, float g, float h, float f, GameObject marker,PathMarker p){
        location = l;
        G = g;
        H = h;
        F = f;
        this.marker = marker;
        parent = p;
        
    }
    public override bool Equals(object obj){
        if((obj == null) || !this.GetType().Equals(obj.GetType())){
            return false;
        }
        else{
            return location.Equals(((PathMarker) obj).location);
        }
    }
    public override  int GetHashCode(){
        return 0;
    }


}

public class FindPathAStar : MonoBehaviour
{
    public Maze maze;
    public Material closedMaterial;
    public Material openMaterial;
    List<PathMarker> open = new List<PathMarker>();
    List<PathMarker> closed = new List<PathMarker>();

    public GameObject start;
    public GameObject end;
    public GameObject pathPoint;
    
    Vector3 goalNode;
    Vector3 startNodePos;
    PathMarker startNode;
    PathMarker endNode;
    PathMarker lastPosition;
    bool comecar = true;
    bool done = false;

    void RemoveAllMarkers(){
        GameObject[] markers = GameObject.FindGameObjectsWithTag("marker");
        foreach(GameObject m in markers){
            Destroy(m);
        }

    }
    void BeginSearch(){
        done= false;
        comecar = true;
        RemoveAllMarkers();
        List<MapLocation> locations = new List<MapLocation>();
        
        
        for(int z = 1; z < maze.depth-1; z++ ){
            for(int x = 1; x < maze.width -1; x++){
                if(maze.map[x,z] != 1){
                    locations.Add(new MapLocation(x,z));
                } //1= walls. 
            }
        }
        locations.Shuffle();
        
        startNodePos = seguidor.transform.position;
      
            //startNodePos = start.transform.position;
        

        //Vector3 startLocation = new Vector3 (locations[0].x * maze.scale, 0, locations[0].z * maze.scale); //y =0 -> flat at least

        

        //PathMarker teste = new PathMarker(new MapLocation(locations[0].x, locations[0].z), 0, 0, 0,
        //                 Instantiate(end, startLocation, Quaternion.identity), null);
       /* if(follows.Count() > 0){
            startNode = follows[0];
        }else{*/
        startNode = new PathMarker(new MapLocation((int)startNodePos.x/ maze.scale, (int)startNodePos.z/ maze.scale),0,0,0,
                                  seguidor, null);

        //}
       
        //Vector3 goalLocation = new Vector3 (locations[1].x * maze.scale, 0, locations[1].z * maze.scale); //y =0 -> flat at least
        goalNode = end.transform.position; 
        endNode = new PathMarker(new MapLocation((int)goalNode.x/maze.scale, (int)goalNode.z/maze.scale),0,0,0,
                                    end, null);
        //here, we've created our start and ending positions and points. 
        open.Clear();
        closed.Clear();
        open.Add(startNode);
        lastPosition = startNode;
        


    }

    void Search(PathMarker thisNode){
        //Debug.Log(thisNode.marker.transform.position.x - goalNode.x);
        //Debug.Log(thisNode.marker.transform.position.z - goalNode.z);
        if (System.Math.Abs(thisNode.marker.transform.position.x - goalNode.x ) < 3 && System.Math.Abs(thisNode.marker.transform.position.z - goalNode.z ) < 3)
        {
            done=true;
            return;
        } //the goal's been found
        //now, loop trough and find all of the neighbours. 
       /* public List<MapLocation> directions = new List<MapLocation>() {
                                            new MapLocation(1,0),
                                            new MapLocation(0,1),
                                            new MapLocation(-1,0),
                                            new MapLocation(0,-1) }; */ //list of neighbours. It's in the Maze code
        foreach(MapLocation dir in maze.directions){
            MapLocation neighbour = dir + thisNode.location;
            if(maze.map[neighbour.x,neighbour.z] == 1 || (neighbour.x < 1 || neighbour.x >= maze.width || neighbour.z < 1 || neighbour.z >= maze.depth )){
                continue;
            } //if it's a wall or if the neighbour is outside the maze realm
            
            
            if(IsClosed(neighbour)){
            
                continue;
            } //if the neighbour is in the list of closed vertexes; 
            float G = Vector2.Distance(thisNode.location.ToVector(), neighbour.ToVector()) + thisNode.G;
            float H = Vector2.Distance(neighbour.ToVector(), goalNode);
            float F = H + G;
            GameObject pathBlock = Instantiate(pathPoint, new Vector3(neighbour.x * maze.scale, 0, neighbour.z * maze.scale), Quaternion.identity);
            TextMesh[] values = pathBlock.GetComponentsInChildren<TextMesh>();  //the G,H and F that shows up in the GameObject. 
            values[0].text = "G: " + G.ToString("0.00");                      
            values[1].text = "H: " + H.ToString("0.00");                      
            values[2].text = "F: " + F.ToString("0.00");                      
            //check if we have to update the 3 values - we can have a neighbour that has already been the neighbour of other visited node, so we have to update the values to this node. 
            if(!UpdateMarker(neighbour, G, H, F, thisNode)){
                open.Add(new PathMarker(neighbour, G,H,F, pathBlock, thisNode));
            }
        }
    
        open = open.OrderBy(p => p.F).ToList<PathMarker>();
        
        PathMarker pm = (PathMarker) open.ElementAt(0);
         
        closed.Add(pm);
        open.RemoveAt(0); 
        pm.marker.GetComponent<Renderer>().material = closedMaterial; 
        lastPosition = pm; //thisNode = pm. 

        
    }
    bool UpdateMarker(MapLocation location, float g, float h, float f, PathMarker prnt){
        foreach(PathMarker p in open){
            if(p.location.Equals(location)){
                p.G = g;
                p.H = h;
                p.F = f;
                p.parent = prnt;
                return true;
            }
        }
        return false;
    }
    
    bool IsClosed(MapLocation marker){
        foreach(PathMarker p in closed){
            if(p.location.Equals(marker)){
                return true;
            }
        }
        return false;
        
    }
    
    List<PathMarker> follows = new List<PathMarker>();    
    GameObject seguidor;
    // Start is called before the first frame update
    void Start()
    {
        seguidor = this.gameObject;
        destino = seguidor.transform.position;
        InvokeRepeating("BeginSearch", 0.01f, 1.5f);
        //BeginSearch();
    }
    
    void GetPath(){
        RemoveAllMarkers();
        follows.Clear();
        PathMarker begin = lastPosition;               
        while(!startNode.Equals(begin) && begin != null){
            Instantiate(pathPoint, new Vector3(begin.location.x * maze.scale, 0 , begin.location.z * maze.scale), Quaternion.identity );
            
            follows.Add(begin);
            //Debug.Log(begin.location.x * maze.scale + " " + begin.location.z * maze.scale + " fazendo array");
            begin = begin.parent;
        }
        follows.Reverse();    
        Instantiate(pathPoint, new Vector3(startNode.location.x * maze.scale, 0 , startNode.location.z * maze.scale), Quaternion.identity );

    }

    void FollowParents(){        
        //Debug.Log(seguidor.transform.position  + "jugador");
        ///Debug.Log(follows[i].location.x * maze.scale + " "+ follows[i].location.z  * maze.scale+ " position to follow");
        //seguidor.transform.position =Vector3.MoveTowards(seguidor.transform.position , new Vector3(follows[i].location.x  * maze.scale, 0,follows[i].location.z  * maze.scale),5); //new Vector3(follows[i].location.x  * maze.scale, 0,follows[i].location.z  * maze.scale); //Vector3.MoveTowards(seguidor.transform.position,new Vector3(follows[i].location.x  * maze.scale, 0,follows[i].location.z  * maze.scale), Time.deltaTime);
        //seguidor.transform.position = new Vector3(follows[i].location.x  * maze.scale, 0,follows[i].location.z  * maze.scale); //new Vector3(follows[i].location.x  * maze.scale, 0,follows[i].location.z  * maze.scale); //Vector3.MoveTowards(seguidor.transform.position,new Vector3(follows[i].location.x  * maze.scale, 0,follows[i].location.z  * maze.scale);
        if(follows.Count() > 0){
            destino = new Vector3(follows[0].location.x  * maze.scale, 0,follows[0].location.z  * maze.scale);        
         //dir * v * Transform.DeltaTime.       
        // lastPosition = follows[0];
        if(follows[0].location.x * maze.scale != (seguidor.transform.position.x )){
                                    Debug.Log("=========================================== ERRO AQUIIIIIIIIIII================================================");
                                //return;
        }
        Debug.Log("Follooows1" + follows[0].location.x + " " + follows[0].location.z);
        follows.RemoveAt(0);
        Debug.Log("Follooows2" + follows[0].location.x + " " + follows[0].location.z);
        Debug.Log(seguidor.transform.position  + "jugador chegou");
        }
                

    }
    int caminhos = 0;
    Vector3 atual ;
    Vector3 destino ;
    // Update is called once per frame
    void Update()
    {
            goalNode = end.transform.position;
            
           /* if(caminhos == 1) {
                destino = new Vector3(follows[0].location.x  * maze.scale, 0,follows[0].location.z  * maze.scale);
                follows.RemoveAt(0);
                seguidor.transform.position = destino;
                

            }*/

            seguidor.transform.LookAt(end.transform);


            if(follows.Count() > 0 && !done){        
                if(!comecar && Vector3.Distance(seguidor.transform.position,  end.transform.position) > 1)
                    seguidor.transform.position = Vector3.MoveTowards(seguidor.transform.position, destino, 0.05f );
                Debug.Log(seguidor.transform.position + " x "+ destino);
                /*Debug.Log(seguidor.transform.position.x + " x X "+ destino.x);
                Debug.Log(seguidor.transform.position.y + " x Y "+ destino.y);
                Debug.Log(seguidor.transform.position.z + " x Z "+ destino.z);*/
                Debug.Log(Vector3.Distance(seguidor.transform.position, destino));
                if(Vector3.Distance(seguidor.transform.position, destino) == 0 ){
                    FollowParents();                    
                    Debug.Log("to na update");
                    
                }

            }
                
            if(comecar && !done){
                Search(lastPosition);           
            }
            if(done){
                done = false;
                comecar = false;
                GetPath();
                caminhos++;
            }        
        
        if(Input.GetKeyDown(KeyCode.P)){
             BeginSearch();
             
        }
           
        if(Input.GetKeyDown(KeyCode.C) && !done){
             Search(lastPosition);
        }
        if(Input.GetKeyDown(KeyCode.M)) GetPath();
        
    }
}

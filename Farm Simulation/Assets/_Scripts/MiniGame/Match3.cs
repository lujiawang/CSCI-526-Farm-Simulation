using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Match3 : MonoBehaviour
{
    public AudioSource matchSound;
    public AudioSource noMatchSound;

    public int Score;
    public int TargetScore;
    public Text ScoreText;
    public Text GoalText;
    public Text TimerText;
    public int CountDown;
    bool takingAway;
    public bool isGameOver;
    public InsertMiniGame game;
    public ArrayLayout boardLayout;
    [Header("UI Elements")]
    public Sprite[] pieces;
    public RectTransform gameBoard;

    [Header("Prefabs")]
    public GameObject nodePiece;

    int width = 14;
    int height = 9;
    Node[,] board;

    List<NodePiece> update;
    List<FlippedPieces> flipped;
    List<NodePiece> dead;

    System.Random random;

    // Start is called before the first frame update
    void Start()
    {
        game = GetComponent<InsertMiniGame>();
        StartGame();
    }

    void Update()
    {
        if(isGameOver)
        {
            enabled = false;
            if(Score >= TargetScore)
                game.OpenCloseMiniGame(true);
            else
                game.OpenCloseMiniGame(false);
        }
        else
        {
            if(takingAway == false && CountDown > 0)
            {
                StartCoroutine(TimerTake());
            }

            if(CountDown == 0)
                isGameOver = true;

            List<NodePiece> finishedUpdating = new List<NodePiece>();
            for(int i = 0; i < update.Count; i++)
            {
                NodePiece piece = update[i];
                if(!piece.UpdatePiece()) finishedUpdating.Add(piece);
            }

            for(int i = 0; i < finishedUpdating.Count; i++)
            {
                NodePiece piece = finishedUpdating[i];
                FlippedPieces flip = getFlipped(piece);
                NodePiece flippedPiece = null;

                List<Point> connected = isConnected(piece.index, true);
                bool wasFlipped = (flip != null);

                if(wasFlipped) //If we flipped to make this update
                {
                    flippedPiece = flip.getOtherPiece(piece);
                    AddPoints(ref connected, isConnected(flippedPiece.index, true));
                }
                if(connected.Count == 0) //If we didn't make a match
                {
                    if(wasFlipped) //If we flipped
                    {
                        FlipPieces(piece.index, flippedPiece.index, false);
                        PlayNoMatch();
                    }
                }
                else //If we made a match
                {
                    int pts = 0;
                    foreach(Point pnt in connected) //Remove the matched nodes 
                    {
                        Node node = getNodeAtPoint(pnt);
                        NodePiece nodePiece = node.getPiece();
                        if(nodePiece != null)
                        {
                            nodePiece.gameObject.SetActive(false);
                            dead.Add(nodePiece);
                        }
                        node.SetPiece(null);
                        pts++;
                    }
                    ApplyGravityToBoard();
                    AddScore(pts);
                    PlayMatch();
                }
                flipped.Remove(flip);
                update.Remove(piece);
            }
            }

    }

    
    public void ApplyGravityToBoard()
    {
        if(isGameOver)
            Debug.Log("Starting to apply Gravity!");
        for(int x = 0; x < width; x++)
        {
            for(int y = (height-1); y >= 0; y--)
            {
                Point p = new Point(x, y);
                Node node = getNodeAtPoint(p);
                int val = getValueAtPoint(p);
                if(val != 0) continue; //If it is not a hole, do nothing
                for(int ny = (y-1); ny >= -1; ny--)
                {
                    Point next = new Point(x, ny);
                    int nextVal = getValueAtPoint(next);
                    if(nextVal == 0) continue;
                    if(nextVal != -1) //If we didn't hit an end
                    {
                        Node got = getNodeAtPoint(next);
                        NodePiece piece = got.getPiece();

                        //Fill up the hole

                        node.SetPiece(piece);
                        update.Add(piece);

                        //Replace the hole
                        
                        got.SetPiece(null);

                    }
                    else // We have hit an end
                    {
                        // Fill in the hole
                        int newVal = fillPiece();
                        NodePiece piece;
                        Point fallPnt = new Point(x, -1);

                        if(dead.Count > 0)
                        {
                            NodePiece revived = dead[0];
                            revived.gameObject.SetActive(true);
                            revived.rect.anchoredPosition = getPositionFromPoint(fallPnt);
                            piece = revived;
                            
                            dead.RemoveAt(0);

                        }
                        else
                        {
                            GameObject obj = Instantiate(nodePiece, gameBoard);
                            NodePiece n =  obj.GetComponent<NodePiece>();
                            RectTransform rect = obj.GetComponent<RectTransform>();
                            rect.anchoredPosition = getPositionFromPoint(fallPnt);
                            piece = n;
                        }

                        piece.Initialize(newVal, p, pieces[newVal - 1]);

                        Node hole = getNodeAtPoint(p);
                        hole.SetPiece(piece);
                        ResetPiece(piece);

                    }
                    break;
                }
            }
        }
              if(isGameOver)
            Debug.Log("Gravity Applied!");
    }

    FlippedPieces getFlipped(NodePiece p)
    {
        FlippedPieces flip = null;
        for(int i = 0; i < flipped.Count; i++)
        {
            if(flipped[i].getOtherPiece(p) != null)
            {
                flip = flipped[i];
                break;
            }
        }

        return flip;
    }

    void StartGame()
    {
        isGameOver = false;
        takingAway = false;
        CountDown = 90; // Change this to adjust count down
        TargetScore = game.price;
        ScoreText.text = "Score: 0";
        GoalText.text = "Goal: " + TargetScore.ToString();
        TimerText.text = CountDown.ToString();
        string seed = getRandomSeed();
        random = new System.Random(seed.GetHashCode());
        update = new List<NodePiece>();
        flipped = new List<FlippedPieces>();
        dead = new List<NodePiece>();
        InitializeBoard();
        VerifyBoard();
        InstantiateBoard();
    }

    void InitializeBoard()
    {
        board = new Node[width, height];
        for(int i = 0; i < height; i++)
        {
            for(int j = 0; j < width; j++)
            {
                board[j,i] = new Node((boardLayout.rows[i].row[j]) ? -1 : fillPiece() , new Point(j, i));
            }
        }
    }

    void VerifyBoard()
    {
        List<int> remove;
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                Point p = new Point(x,y);
                int val = getValueAtPoint(p);
                if(val <= 0) continue;
                
                remove = new List<int>();
                while(isConnected(p, true).Count > 0)
                {
                    val = getValueAtPoint(p);
                    if(!remove.Contains(val))
                        remove.Add(val);
                    setValueAtPoint(p, newValue(ref remove));
                }

            }
        }
    }

    void InstantiateBoard()
    {
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                Node node = getNodeAtPoint(new Point(x , y));

                int val = node.value;
                if(val <= 0 ) continue;
                GameObject p = Instantiate(nodePiece, gameBoard);
                NodePiece piece = p.GetComponent<NodePiece>();
                RectTransform rect = p.GetComponent<RectTransform>();
                rect.anchoredPosition = new Vector2(48 + (96 * x), -48 - (96 * y));
                piece.Initialize(val, new Point(x, y), pieces[val - 1]);
                node.SetPiece(piece); 
            }
        }

    }

    public void ResetPiece(NodePiece piece)
    {
        piece.ResetPosition();
        update.Add(piece);

    }

    public void FlipPieces(Point one, Point two, bool main)
    {
        if(getValueAtPoint(one) < 0) return;
        Node nodeOne = getNodeAtPoint(one);
        NodePiece pieceOne = nodeOne.getPiece();
        if(getValueAtPoint(two) > 0)
        {
            Node nodeTwo = getNodeAtPoint(two);
            NodePiece pieceTwo = nodeTwo.getPiece();
            nodeOne.SetPiece(pieceTwo);
            nodeTwo.SetPiece(pieceOne);

            if(main)
                flipped.Add(new FlippedPieces(pieceOne, pieceTwo));

            update.Add(pieceOne);
            update.Add(pieceTwo);

        }
        else 
            ResetPiece(pieceOne);
    }

    List<Point> isConnected(Point p, bool main)
    {
        List<Point> connected = new List<Point>();
        int val = getValueAtPoint(p);
        Point[] directions = 
        {
            Point.up,
            Point.right,
            Point.down, 
            Point.left
        };

        foreach(Point dir in directions) // Check if there is 2 or more same shapes in the directions
        {
            List<Point> line = new List<Point>();

            int same = 0;
            for(int i = 1; i < 3; i++)
            {
                Point check = Point.add(p, Point.mult(dir, i));
                if(getValueAtPoint(check) == val)
                {
                    line.Add(check);
                    same++;
                }
            }
            if(same > 1)
            {
                AddPoints(ref connected, line);
            }
        }

        for(int i = 0; i < 2; i++) // Check if it is in the middle of two of the same shapes
        {
            List<Point> line = new List<Point>();

            int same = 0;
            Point[] check = { Point.add(p, directions[i]), Point.add(p, directions[i+2]) };
            foreach(Point next in check)
            {
                if(getValueAtPoint(next) == val)
                {
                    line.Add(next);
                    same++;
                }
            }

            if(same > 1)
                AddPoints(ref connected, line);
        }

        for(int i = 0; i < 4; i++) // Check for 2x2
        {
            List<Point> square = new List<Point>();

            int same = 0;
            int next = i + 1;
            if(next >= 4)
                next -= 4;
            
            Point[] check = { Point.add(p, directions[i]), Point.add(p, directions[next]), Point.add(p, Point.add(directions[i],
                directions[next])) };

            foreach(Point pnt in check)
            {
                if(getValueAtPoint(pnt) == val)
                {
                    square.Add(pnt);
                    same++;
                }
            }
            if(same > 2)
                AddPoints(ref connected, square);

        }

        if(main)
        {
            for(int i = 0; i < connected.Count; i++)
            {
                AddPoints(ref connected, isConnected(connected[i], false));
            }
        }
 
        return connected;
    }

    void AddPoints(ref List<Point> points, List<Point> add)
    {
        foreach(Point p in add)
        {
            bool doAdd = true;
            for(int i = 0; i < points.Count; i++)
            {
                if(points[i].Equals(p))
                {
                    doAdd = false;
                    break;
                }
            }

            if(doAdd) points.Add(p);
        }
    }

    int fillPiece()
    {
        int val = (random.Next(0,100) / (100 / pieces.Length)) + 1;
        return val;
    }

    int getValueAtPoint(Point p)
    {
        if(p.x < 0 || p.x >= width || p.y < 0 || p.y >= height) return -1;
        return board[p.x, p.y].value;
    }

    void setValueAtPoint(Point p, int v)
    {
        board[p.x, p.y].value = v;
    }

    Node getNodeAtPoint(Point p)
    {
        return board[p.x, p.y];
    }

    int newValue(ref List<int> remove)
    {
        List<int> available = new List<int>();
        for(int i = 0; i < pieces.Length; i++)
            available.Add(i + 1);
        foreach (int i in remove)
            available.Remove(i);

        if(available.Count <= 0) return 0;
        return available[random.Next(0, available.Count)];
    }


    string getRandomSeed()
    {
        string seed = "";
        string acceptableChars = "ABCDEFGHIJKLMNOPQRSTUVWXYAabcdefghijklmnopqrstuvwxyz1234567890!@#$%^&*()";
        for(int i = 0; i < 20; i++)
        {
            seed += acceptableChars[Random.Range(0, acceptableChars.Length)];
        }
        return seed;
    }

    public Vector2 getPositionFromPoint(Point p)
    {
        return new Vector2(48 + (96 * p.x), (-48 - (96 * p.y)));
    } 

    public void AddScore(int pts)
    {
        Score += pts;
        ScoreText.text = "Score: " + Score.ToString();
        if(Score >= TargetScore)
        {
            isGameOver = true;
        }
            
    }

    IEnumerator TimerTake()
    {
        takingAway = true;
        yield return new WaitForSeconds(1);
        CountDown -= 1;
        TimerText.text = CountDown.ToString(); 
        takingAway = false;
    }

    public void PlayMatch()
    {
        matchSound.Play();
    }

    public void PlayNoMatch()
    {
        noMatchSound.Play();
    }

}


[System.Serializable]
public class Node
{
    public int value;
    public Point index;
    NodePiece piece;

    public Node(int v, Point i)
    {
        value = v;
        index = i;
    }

    public void SetPiece(NodePiece p)
    {
        piece = p;
        value = (piece == null) ? 0 : piece.value;
        if(piece == null) return;
        piece.SetIndex(index);
    }

    public NodePiece getPiece()
    {
        return piece;
    }
}


[System.Serializable]
public class FlippedPieces
{
    public NodePiece one;
    public NodePiece two;

    public FlippedPieces(NodePiece o, NodePiece t)
    {
        one = o;
        two = t;

    }

    public NodePiece getOtherPiece(NodePiece p)
    {
        if(p == one)
            return two;
        else if(p == two)
            return one;
        else
            return null;
    }


}
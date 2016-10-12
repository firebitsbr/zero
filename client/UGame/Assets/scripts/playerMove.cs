﻿using UnityEngine;
using System.Collections;

public class playerMove : MonoBehaviour {
	private bool isMoving;
	//private Vector3 beginxy;
	private Vector3 endxy;
	public float speed;
	public float step;
	// Use this for initialization

	private GameObject map;

	//player初始位置
	private int[] iniCell;
	private Vector2 iniPos;

	//player动画控制
	private Animator animator; 
	private int pathid;
	//存储player的行，列，朝向；在移动的时候变化
	private int row;
	private int column;
	private string orientation;


	private Astar astar;
	void Awake(){
		map = GameObject.Find ("map");
		OBJTYPEList obj_list  = map.GetComponent<RandomDungeonCreator>().obj_list;//获取object列表
		row = obj_list.getListByType (OBJTYPE.OBJTYPE_SPAWNPOINT) [0].row;
		column = obj_list.getListByType (OBJTYPE.OBJTYPE_SPAWNPOINT) [0].column;
		orientation = "DOWN";
		iniCell = new int[2];
		iniCell [0] = row;
		iniCell [1] = column;
		iniPos = map.GetComponent<TilesManager>().posTransform(row,column);
		//初始化位置
		transform.position = iniPos;
		astar= new Astar();

	}

	void Start () {
		isMoving = false;
		endxy = transform.position;
		animator = GetComponent<Animator>();


	}

	//根据单元格做碰撞检测
	private void AttemptMove(string dir,int i,int j){
		string tileType = map.GetComponent<RandomDungeonCreator>().getMapTileType(i,j);
		switch (tileType) {
			default:

				if (isMoving)
					return;
				switch (dir) {
				case "UP":
					endxy = new Vector3 (transform.position.x, transform.position.y + step, 0);
					row--;
//				Debug.Log (row + "," + column + " UP");
					break;
				case "DOWN":
					endxy = new Vector3 (transform.position.x, transform.position.y - step, 0);
					row++;
//				Debug.Log (row + "," + column + " DOWN");
					break;
				case "LEFT":
					endxy = new Vector3 (transform.position.x - step, transform.position.y, 0);
					column--;
//				Debug.Log (row + "," + column + " LEFT");
					break;
				case "RIGHT":
					endxy = new Vector3 (transform.position.x + step, transform.position.y, 0);
					column++;
//				Debug.Log (row + "," + column + " RIGHT");
					break;
				}
				isMoving = true;
			break;
			case "WALL":
				Debug.Log ("cnot go");
			break;

		}
		return;
	}

	public void moveUp(){
		orientation = "UP";
		Debug.Log ("UP");
		animator.SetTrigger ("PlayerMoveUp");
		AttemptMove (orientation,row - 1, column);
	
	}
	public void moveDown(){
		orientation = "DOWN";
		//Debug.Log ("Down");
		animator.SetTrigger ("PlayerMoveDown");
		AttemptMove (orientation,row+1, column);

	}
	public void moveLeft(){
		orientation = "LEFT";

		animator.SetTrigger ("PlayerMoveLeft");
		AttemptMove (orientation,row, column-1);

	}
	public void moveRight(){
		orientation = "RIGHT";
		Debug.Log ("Right");
		animator.SetTrigger ("PlayerMoveRight");
		AttemptMove (orientation,row, column+1);
	}
	// Update is called once per frame
	void Update () { 
		Vector3	screenPosition = Camera.main.WorldToScreenPoint(transform.position);  
		Vector3 mousePositionOnScreen = Input.mousePosition;   
		mousePositionOnScreen.z = screenPosition.z;  
		Vector3	mousePositionInWorld =  Camera.main.ScreenToWorldPoint(mousePositionOnScreen);  
		if (Input .GetMouseButtonDown(0)) {  
			int[] pos=map.GetComponent<TilesManager>().posTransform2(mousePositionInWorld.x,mousePositionInWorld.y);
			astar= new Astar(row,column,pos[0],pos[1],map.GetComponent<RandomDungeonCreator>().getMap(),32,32);
			astar.Run ();
//			Debug.Log ("Path long = " + astar.finalpath.Count);
			pathid = astar.finalpath.Count-1;
			if (pathid >= 1) {
//				Debug.Log ("path"+pathid+":"+row + "," + column + " to " + astar.finalpath [pathid] [0] + "," + astar.finalpath [pathid] [1]);
				if (astar.finalpath [pathid] [0] < row)
					moveUp ();
				if (astar.finalpath [pathid] [0] > row)
					moveDown ();
				if (astar.finalpath [pathid] [1] < column)
					moveLeft ();
				if (astar.finalpath [pathid] [1] > column)
					moveRight ();
			}
			//Instantiate (, mousePositionInWorld , Quaternion.identity); 

			Debug.Log (pos[0]+","+pos[1]);
		}  
		if (isMoving) {
			transform.position = new Vector3 (Mathf.MoveTowards (transform.position.x, endxy.x, Time.deltaTime * speed), Mathf.MoveTowards (transform.position.y, endxy.y, Time.deltaTime * speed), 0);
			GameObject.Find ("light").GetComponent<ligthmap> ().reDrawLight ();
		}
		if (transform.position == endxy) {
			
			transform.position = endxy;
			pathid--;
			if (pathid < 0  && isMoving) {
				isMoving = false;
//				Debug.Log (orientation);
//				根据朝向设置 player的动画
				switch (orientation){
				case "UP": 
					animator.SetTrigger ("PlayerIdleUp");
					break;
				case "DOWN": 
					animator.SetTrigger ("PlayerIdleDown");
					break;
				case "LEFT": 
					animator.SetTrigger ("PlayerIdleLeft");
					break;
				case "RIGHT": 
					animator.SetTrigger ("PlayerIdleRight");
					break;
				default:
					animator.SetTrigger ("PlayerIdleDown");
					break;
				}

			}
			else if(pathid>=0){
				isMoving = false;
//				Debug.Log ("path"+pathid+":"+row + "," + column + " to " + astar.finalpath [pathid] [0] + "," + astar.finalpath [pathid] [1]);
				if (astar.finalpath [pathid] [0] < row)
					moveUp ();
				if (astar.finalpath [pathid] [0] > row)
					moveDown ();
				if (astar.finalpath [pathid] [1] < column)
					moveLeft ();
				if (astar.finalpath [pathid] [1] > column)
					moveRight ();
				//Debug.Log (transform.position.x + "," + transform.position.y + " " + endxy.x + "," + endxy.y);
			}

		}

	}
}

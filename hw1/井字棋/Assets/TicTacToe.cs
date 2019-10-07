using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class TicTacToe : MonoBehaviour {
    public int turn;//表示轮到谁下期
    public int count;//计算是否是平局
    public int players = 2;
    private int[,] cells = new int[3, 3];//储存九个格子的状态
    public Texture2D img;//背景

	void Start () {
        restart();
	}

    void restart() {
        turn = 1;
        for (int i = 0; i < 3; ++i) {
            for (int j = 0; j < 3; ++j) {
                cells[i, j] = 0;
            }
        }
        count = 0;
    }

    private int winCheck() {
        for(int i = 0; i < 3; ++i) {
            if (cells[i, 0] != 0 && cells[i,0]==cells[i, 1] && cells[i, 1] == cells[i, 2]) {
                return cells[i, 0];
            }
        }

        for (int i = 0; i < 3; ++i) {
            if (cells[0, i] != 0 && cells[0, i] == cells[1, i] && cells[1, i] == cells[2, i]) {
                return cells[0, i];
            }
        }

        if(cells[1,1] != 0 && cells[0,0] == cells[1,1] && cells[1,1] == cells[2,2] || cells[0,2] == cells[1,1] && cells[1,1] == cells[2,0]) {
            return cells[1, 1];
        }
        if (count >= 9) 
            return 3;

        return 0;
    }

    int checkForNext(int x, int y){
        if(cells[(x+1)%3, y] == 1 && cells[(x+2)%3, y] == 1)
            return 1;
        if(cells[x, (y+1)%3] == 1 && cells[x, (y+2)%3] == 1)
            return 1;

        if(cells[(x+1)%3, y] == 2 && cells[(x+2)%3, y] == 2)
            return 2;
        if(cells[x, (y+1)%3] == 2 && cells[x, (y+2)%3] == 2)
            return 2;

        if(x==y) {
            if(cells[(x+1)%3, (y+1)%3] == 1 && cells[(x+2)%3, (y+2)%3] == 1)
                return 1;
            if(cells[(x+1)%3, (y+1)%3] == 2 && cells[(x+2)%3, (y+2)%3] == 2)
                return 2;
        }

        if(x+y==2) {
            if(cells[(x+1)%3, (y+2)%3] == 1 && cells[(x+2)%3, (y+1)%3] == 1)
                return 1;
            if(cells[(x+1)%3, (y+2)%3] == 2 && cells[(x+2)%3, (y+1)%3] == 2)
                return 2;
        }

        return 0;
    }


    private void OnGUI()
    {   
        GUIStyle temp1 = new GUIStyle {
            fontSize = 50
        };
        temp1.normal.textColor = Color.white;
        temp1.fontStyle = FontStyle.BoldAndItalic;

        GUIStyle temp2 = new GUIStyle {
            fontSize = 20
        };
        temp2.normal.textColor = Color.red;
        temp2.fontStyle = FontStyle.BoldAndItalic;

        //背景图片
        GUIStyle temp3 = new GUIStyle();
        temp3.normal.background = img;
        GUI.Label (new Rect (0, 0, 800, 500), "", temp3);


        GUI.Label(new Rect(420, 20, 100, 50), "TicTacToe", style: temp1);
        if(GUI.Button(new Rect(500, 220, 100, 50),"restart"))
        {
            restart();
        }

        //调换双人或担任模式
        if(GUI.Button(new Rect(500, 150, 100, 50),""))
        {
            players = 3 - players;
            restart();
        }
        if(players==2)
        {
            GUI.Label(new Rect(520, 165, 100, 50), "1 players");
        } 
        if(players==1)
        {
            GUI.Label(new Rect(520, 165, 100, 50), "2 players");
        } 

        int result = winCheck();//
        Debug.Log(count);
        switch (result)
        {
            case 1:
                GUI.Label(new Rect(520, 100, 100, 50), "O WIN", style: temp2);//先手赢;
                break;
            case 2:
                GUI.Label(new Rect(520, 100, 100, 50), "X WIN", style: temp2);//后手胜
                break;
            case 3:
                GUI.Label(new Rect(520, 100, 100, 50), "DUAL", style: temp2);//平局
                break;
        }

        //单人模式
        if(players==1 && turn == -1){
            int flag = 0;
            for(int i = 0; i < 3; i++){
                for(int j = 0; j < 3; j++){
                    if(cells[i,j] == 0){
                        //Debug.Log("here!");
                        int r = checkForNext(i,j);
                        if(r!=0){
                            cells[i,j] = 2;
                            i = 3;
                            j = 3;
                            flag = 1;
                        }
                    }
                    
                }
            }
            if(flag == 0){
                if(cells[1,1] == 0){
                    cells[1,1] = 2;
                }
                else{
                    for(int i = 0; i < 3; i++)
                        for(int j = 0; j < 3; j++){
                            if(cells[i,j] == 0){
                                cells[i,j] = 2;
                                i = 3;
                                j = 3;
                            }
                        }     
                }
            }
            turn = 1;
            count++;
        }

        for (int i = 0; i < 3; ++i)
        {
            for(int j = 0; j < 3; ++j)
            {
                if (cells[i, j] == 1)
                {
                    GUI.Button(new Rect(50 + i * 80, 30 + j * 80, 80, 80), "0");
                }
                if (cells[i, j] == 2)
                {
                    GUI.Button(new Rect(50 + i * 80, 30 + j * 80, 80, 80), "X");
                }
                if(GUI.Button(new Rect(50 + i * 80, 30 + j * 80, 80, 80), ""))
                {
                    if (result == 0)
                    {
                        if (turn == 1) cells[i, j] = 1;
                        else cells[i, j] = 2;
                        count++;
                        turn = -turn;
                    }
                }
            }
        }
    }

}
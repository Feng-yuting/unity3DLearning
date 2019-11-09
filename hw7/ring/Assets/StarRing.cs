using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleInfo
{
    public float radius = 0;
    public float angle = 0;
	
    public ParticleInfo(float radius, float angle)
    {
        this.radius = radius;   // 粒子半径  
        this.angle = angle;     // 粒子角度  
    }
}

public class StarRing : MonoBehaviour {
    private ParticleSystem particleSys;  // 粒子系统  
    private ParticleSystem.Particle[] particleArr;  // 所有粒子  
    private ParticleInfo[] info; // 所有粒子信息  

    float speed = 0.25f;            // 粒子速度    
    public int count = 8000;       // 粒子数量 

    void Start () {
        // 初始化 
        particleArr = new ParticleSystem.Particle[count];
        info = new ParticleInfo[count];
 
        particleSys = this.GetComponent<ParticleSystem>();
        particleSys.loop = false;              // 取消循环
        particleSys.startSpeed = 0;            // 设置粒子初速度      
        particleSys.maxParticles = count;      // 设置最大粒子量  
        particleSys.Emit(count);               // 发射粒子  
        particleSys.GetParticles(particleArr); // 获取所有粒子

        IniAll();   // 初始化所有粒子
    }

    // Update is called once per frame 
    void Update()
    {
        for (int i = 0; i < count; i++)
        {
            // 让速度在小幅度内波动
            float rotateSpeed = (speed / info[i].radius) * (i % 10 + 1);
			
			// 移动
            info[i].angle += rotateSpeed;
                        

            // 保证角度合法
            info[i].angle %= 360.0f;
            // 转换成弧度制
            float radian = info[i].angle * Mathf.PI / 180;

            // 粒子在半径方向上抖动           
            float offset = Random.Range(-0.01f, 0.01f);  // 偏移范围
            info[i].radius += offset;

            particleArr[i].position = new Vector3(info[i].radius * Mathf.Cos(radian), 0f, info[i].radius * Mathf.Sin(radian));
        }
        // 通过粒子数组设置粒子系统
        particleSys.SetParticles(particleArr, particleArr.Length);
    }

    void IniAll()
    {          
        float minRadius = 6.0f;  // 最小半径  
        float maxRadius = 10.0f; // 最大半径           
        for (int i = 0; i < count; ++i)
        {   
            // 随机每个粒子半径，集中于平均半径附近  
            float midRadius = (maxRadius + minRadius) / 2;
            float minRate = Random.Range(1.0f, midRadius / minRadius);
            float maxRate = Random.Range(midRadius / maxRadius, 1.0f);
            float radius = Random.Range(minRadius * minRate, maxRadius * maxRate);

            // 随机每个粒子的角度  
            float angle = Random.Range(0, 360);
            // 转换成弧度制
            float radian = angle / 180 * Mathf.PI;

            // 随机每个粒子的大小
            float size = Random.Range(0.01f, 0.03f);

            info[i] = new ParticleInfo(radius, angle);            

            particleArr[i].position = new Vector3(info[i].radius * Mathf.Cos(radian), 0f, info[i].radius * Mathf.Sin(radian));
            particleArr[i].size = size;            
        }
        // 通过初始化好的粒子数组设置粒子系统
        particleSys.SetParticles(particleArr, particleArr.Length);
    }
}

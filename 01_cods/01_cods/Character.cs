﻿// using : 어떤 추가적인 기능을 사용할 것인지를 표시하는것
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

// namespace : 이름이 겹치는 것을 방자하기 위해 구분지어 놓는 용도
namespace _01_cods
{
    // 접근제한자(Access Modifier) 
    // public : 누구나 접근할 수 있다.
    // private : 자기 자신만 접근할 수 있다.
    // protected : 자신과 자신을 상속받은 자식만 접근할 수 있다.
    // internal : 같은 어셈블리안에서는 public 다른 어셈블리면 private

    // 클래스 : 특정한 오브젝트를 표현하기 위해 그 오브젝트가 가져야 할 데이터와 기능을 모아 놓은 것
   public class Character    // Character 클래스를 public으로 선언한다.
    {
        // 맴버 변수 -> 이 클래스에서 사용되는 데이터.
        protected string name;
        protected int hp = 100;
        protected int maxHP = 100;
        protected int STR = 10;
        protected int DEX = 5;
        protected int intellegence = 7;

        bool isDead = false;
        public bool barrier { get; set; }


        public string Name => name;

        public bool IsDead => isDead;   // 간단하게 읽기전용 프로퍼티 만드는 방법

        //Random random = new Random();
        //for(int i = 1; i < 101; i++)
        //{
        //int randNum = random.Next();
        //    // % : 앞에 숫자를 뒤의 숫자로 나눈 나머지값을 돌려주는 연산자. (모듈레이트 연산, 나머지 연산)
        //    // 10 % 3 하면 결과는 0~2
        //    // % 연산의 결과는 항상 0~(뒤 숫자 -1)로 나온다.
        //Console.WriteLine($"랜덤 넘버: {randNum}");
        //}




        // 배열 : 같은 종류(데이터 타입)의 데이터를 한번에 여러개 가지는 유형의 변수
        // int[] arr;  // 인터지러를 여러개 가질 수 있는 배열
        // arr = new int[5];       // 인티저를 5개 가질 수 있도록 할당

        string[] nameArray = { "현진", "준형", "사빈", "원빈", "상현" };
        // nameArray에 기본값 설정(선언과 할당을 동시해 함)

        protected Random rand;

        public int HP
        {
            get // 이 privat를 읽을 때 호출되는 부분. get만 만들면 읽기 전용 같은 효과가 있다.
            {
                return hp;
            }
            private set // 이 privat를 값을 넣을 때 호출되는 부분. set에 private을 붙이면 쓰는 것은 나만 가능하다.
            {
                hp = value;
                if (hp > maxHP)
                    hp = maxHP;
                if(hp <= 0)
                {
                    // 사망 처리용 함수 호출
                    hp = 0;
                    isDead = true;
                    Console.WriteLine($"{name}이 사망하였습니다.");
                }
            }
        }

        public Character()
        {
            //Console.WriteLine("생성자 호출");

            // 실습
            // 1. 이름이 nameArray에 들어있는 것 중 하나로 랜덤하게 선택된다.
            // 2. maxHp는 100~200로 랜덤하게 선택된다.
            // 3, hp는 maxHp와 같은 값이다.
            // 4. STR, DEX, INT은 1~20 사이로 랜덤하게 정해진다.
            // 5. TestPrintStatus 함수를 이용해서 초죙 상태를 출력한다.

            rand = new Random(DateTime.Now.Millisecond);
            int randNum = rand.Next();      // 랜덤 클래스 이용해서 0~21억 사이의 숫자를 랜덤으로 선택
            name = nameArray[randNum % 5];  // 랜덤으로 고른 숫자를 0~4로 변경

            GenerateStatus();
            TestPrintStatus();

        }

        public Character(string newName)
        {
            //Console.WriteLine($"생성자 호출 - {newName}");
            name = newName;
            rand = new Random(DateTime.Now.Millisecond);
            GenerateStatus();
            TestPrintStatus();
        }

        public virtual void GenerateStatus()
        {
            maxHP = rand.Next(100, 201);    // 100에서 200중에 랜덤으로 선택
            hp = maxHP;

            STR = rand.Next(20) + 1;         // 1~20 사이를 랜덤하게 선택
            DEX = rand.Next(20) + 1;
            intellegence = rand.Next(20) + 1;
        }

        // 맴버 함수 -> 이 클래스가 가지는 기능
        public virtual void Attack(Character target)
        {
            
        }

        public void TakeDamge(int damage)
        {
            Console.WriteLine($"{name}님이 {damage}데미지를 받았습니다.");
            HP -= damage;
        }

        public virtual void TestPrintStatus()
        {
            
        }

        
    }
}

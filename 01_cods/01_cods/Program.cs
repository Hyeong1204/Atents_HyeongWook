using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace _01_cods
{
    internal class Program
    {
        // 스코프(scope) : 변수나 함수를 사용할 수 있는 범위. 변수를 선어한 시점에서 해당 변수가 포한된 중괄호가 끝나는 구간까지
        static void Main(string[] args)
        {
            // 주말과제용
            Human player;       // Human 클래스 player를 선언
            Console.WriteLine("당신의 이름을 입력해주세요 : ");
            string NewName = Console.ReadLine();    // string타입 NewName에 입력한 값을 받는다.
            string selc;
            do
            {
                player = new Human(NewName);    // Human타입에 player를 만들고 NewName 값을 대입한다., 스테이터스 랜덤으로 설정
                Console.WriteLine("이대로 진행하겠습니까? (yes/no) : ");
                selc = Console.ReadLine();      // string타입 selcs에 입력한 값을 받는다.
            } while (selc != "yes" && selc != "Yes" && selc != "y" && selc != "Y");     // yes, Yes, y, Y 값을 받으면 do ~ while문을 빠저나온다.

            // 적 만들기
            Orc ememy = new Orc("오크");      // Ore 클래스 ememy를 선언과 동시에 Orc타입으로 만든다., 오크라는 이름으로 만들기

            Console.WriteLine($"{ememy.Name}가 나타났다.");
            Console.WriteLine("\n\n--------------------------------전투시작--------------------------------\n\n");


            while (true)    // 무한 루프(전퉅가 끝날때까지, 한명이 죽으면 break로 종료)
            {
                int n;      // int형 n변수를 선언
                
                do
                {
                    Console.WriteLine("┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓");
                    Console.WriteLine("┃ \t   커맨드를 입력하세요.\t\t┃");
                    Console.WriteLine("┃ 1. 공격\t2. 휘두르기\t3. 방어 ┃");
                    Console.WriteLine("┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");
                    string temp = Console.ReadLine();       // 입력
                    int.TryParse(temp, out n);              // temp변수를 정수형으로 바꿈
                } while (n < 1 || n > 3);                   // n의 값이 1 ~ 3 이면 do ~ while문을 나온다.

                switch (n)
                {
                    case 1:     // n이 1이면
                        player.Attack(ememy);       // player에 Attack함수를 호출하고 (ememy의 인자값을 넣는다)
                        break;      // break : 스위치문 탈출
                    case 2:     // n이 2이면
                        player.HumanSkill(ememy);   // player에 HumanSkill함수를 호출하고 (ememy의 인자값을 넣는다)
                        break;
                    case 3:     // n이 3이면
                        player.Defaense();          // player에 Defaense 함수를 호출
                        break;
                    default:    // n이 그 무엇도 아닐때
                        break;
                }

                player.TestPrintStatus();       // player에 TesPrintStatus 함수를 호출
                ememy.TestPrintStatus();        // ememy에 TesPrintStatus 함수를 호출
                if (ememy.IsDead)               // ememy가 죽으면
                {
                    Console.WriteLine("승리!");   
                    break;             // while문 탈출
                }
                ememy.Attack(player);           // ememy에 Attack함수를 호출하고 (player의 인자값을 넣는다)
                player.TestPrintStatus();       // player에 TesPrintStatus 함수를 호출
                ememy.TestPrintStatus();        // ememy에 TesPrintStatus 함수를 호출
                if (player.IsDead)              // player가 죽으면
                {
                    Console.WriteLine("패배!");
                    break;              // while문 탈출
                }
            }
            
            //while(!player.IsDead && !ememy.IsDead)      // 둘중에 한명이 죽을때 까지 반복
            //{
            //    Console.WriteLine("┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓");
            //    Console.WriteLine("┃ \t   커맨드를 입력하세요.\t\t┃");
            //    Console.WriteLine("┃ 1. 공격\t2. 휘두르기\t3. 방어 ┃");
            //    Console.WriteLine("┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");

            //    string temp = Console.ReadLine();       // 입력
            //    int.TryParse(temp, out n);              // temp변수를 정수형으로 바꿈


            //    switch (n)
            //    {
            //        case 1:
            //            player.Attack(ememy);           // player에 Attack 함수를 호출
            //            break;
            //        case 2:
            //            player.HumanSkill(ememy);       // palyer에 HumanSkill 함수를 호출
            //            break;
            //        case 3:
            //            player.Barrier = true;          // palyer에 Barrier를 false에서 true로 변경
            //            break;
            //        default:
            //            Console.WriteLine("다시 입력해주세요. ");
            //            continue;           // 다시 while문 시작점으로 이동
            //    }
            //    player.TestPrintStatus();
            //    ememy.TestPrintStatus();
            //    if (ememy.IsDead)           // ememy가 죽었는지 한번더 검출
            //    {
            //        break;
            //    }
            //    ememy.Attack(player);
            //    player.TestPrintStatus();
            //    ememy.TestPrintStatus();
            //}

            Console.ReadKey();                  // 키 입력 대기하는 코드
        }   // Main 함수의 끝

       
    }
}

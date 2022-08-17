using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _01_cods
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");  // "Hellp World"를 출력하는 코드
            Console.WriteLine("임형욱");        // 출력 
            //string str = Console.ReadLine();    // 키보드 입력을 받아서 str이라는 string 변수에 저장한다.

            // 변수 : 변하는 숫자, 컴퓨터에서 사용할 데이터를 저장할 수 있는 곳.
            // 변수의 종류 : 데이터 타입(Data type), 32bit
            // int : 인티저, 정수, 소수점 없는 숫자 , 32bit
            // folat : 플로트, 실수, 소수점 있는 숫자
            // string : 스트링, 문자열, 글자들을 저장
            // bool : 불, 참 또는 거짓를 저장

            //int a = 10; // a라는 인티저 변수에 10이라는 데이터를 넣는다.
            //long b = 5000000000;    // 50억은 int에 넣을 수 없다. =>int는 32비트이고 32비트로
            //                        // 표현가능한 숫자의 갯수는 2^32(42억)개 이기 때문이다.
            //int c = -100;
            //int d = 2000000000;
            //int e = 2000000000;
            //int f = d + e;

            //Console.WriteLine(f);


            //// float의 단점 : 태생적으로 오차가 있을 수 밖에 없다.
            //float aa = 0.000123f;
            //float ab = 0.99999999999f;
            //float ac = 0.00000000001f;

            //Console.WriteLine(ab + ac); // 결과가 1이 아닐 수도 있다.

            //string str1 = "Hello";
            //string str2 = "안녕!";
            //string str3 = $"Hello {a}"; // "Hello 10"
            //Console.WriteLine(str3);
            //string str4 = str1 + str2;  // "Hello안녕!"
            //Console.WriteLine(str4);

            //bool b1 = true;
            //bool b2 = false;

            //int level = 1;
            //int hp = 10;
            //float exp = 0.9f;
            //string name = "황꾸릉";

            //// 황꾸릉의 레벨은 1이고 HP는 10이고 exp는 0.9다.

            //Console.Write(name);
            //Console.Write("의 레벨은 ");
            //Console.Write(level);
            //Console.Write("이고 HP는 ");
            //Console.Write(hp);
            //Console.Write("이고 exp는 ");
            //Console.Write(exp);
            //Console.Write("다.");
            //Console.WriteLine();

            //Console.WriteLine($"{name}의 레벨은 {level}이고 HP는 {hp}이고 exp는 {exp}다.\n");

            //Console.WriteLine("이름을 입력하세요 :");
            //name = Console.ReadLine();
            //Console.WriteLine($"{name}의 레벨을 입력하세요 : ");
            //string temp = Console.ReadLine();   // ReadLine은 문자열만 가능하다.
            ////level = int.Parse(temp);    // string을 int로 변겨해주는 코드(숫자로 바꿀 수 있을때만 가능) 간단하지만 위험함
            //int.TryParse(temp, out level); // string을 int로 변경해주는 코드(숫자로 바꿀 수 없을 때는 0으로 만든다.)
            //                               //level = Convert.ToInt32(temp);  // string을 int로 변겨해주는 코드(숫자로 바꿀 수 있을때만 가능) 더 새새하게 가능 간단하지만 위험함
            //Console.WriteLine("HP를 입력하세요 : ");
            //string temp2 = Console.ReadLine();
            //int.TryParse(temp2, out hp);
            //Console.WriteLine("경험치를 입력하세요 : ");
            //string temp3 = Console.ReadLine();
            //float.TryParse(temp3, out exp);
            //Console.WriteLine($"{name}의 레벨은 {level}이고 HP는 {hp}이고 exp는 {exp*100:f2}%다.\n");
            //// exp*100:f2는 exp에 100을 곱하고 소수 두번째 자리까지 표시

            string result;
            string name = "황꾸릉";
            int level = 3;
            int hp = 2;
            float exp = 0.5f;

            //Console.Write("이름을 입력하세요 : ");
            //name = Console.ReadLine();

            //string temp;
            //Console.Write("레벨을 입력하세요 : ");
            //temp = Console.ReadLine();
            //int.TryParse(temp, out level);

            //string temp2;
            //Console.Write("hp를 입력하세요 : ");
            //temp = Console.ReadLine();
            //int.TryParse(temp, out hp);

            //string temp3;
            //Console.Write("exp를 입력하세요 : ");
            //temp = Console.ReadLine();
            //float.TryParse(temp, out exp);

            //Console.WriteLine($"{name}의 레벨은 {level}이고 HP는 {hp}이고 exp는 {exp * 100:f2}%다.\n");

            // 변수 끝 ------------------------------------------------------------------------

            // 제어문(Control state) - 조건문(if, switch), 반복문
            // 실행되는 코드 라인을 변경할 수 있는 코드

            // 조건문 실습
            //hp = 10;
            //if (hp < 3)     // hp가 2이기 때문에 참이다. 따라서 중괄호 사이에 코드가 실행된다.
            //{
            //    Console.WriteLine("hp가 부족합니다.");    // 참일 때 실행되는 코드
            //}
            //else if(hp < 10)
            //{
            //    Console.WriteLine("hp가 적당합니다.");    // (hp < 3)는 거짓이고 (hp < 10)이 참일때
            //}
            //else
            //{
            //    Console.WriteLine("hp가 충분합니다.");    // 둘다 거짓일 때 실행되는 코드
            //}

            //switch (hp)
            //{
            //    case 0:
            //        Console.WriteLine("hp가 0입니다.");
            //        break;
            //    case 5:
            //        Console.WriteLine("hp가 5입니다.");
            //        break;
            //    default:
            //        Console.WriteLine("hp가 0과 5가 아닙니다.");
            //        break;
            //}

            //Console.WriteLine("경험치를 추가합니다.");
            //Console.Write("추가할 경험치 : ");
            //string temp = Console.ReadLine();

            //// 실습 : exp의값과 추가로 입력받은 경험치의 합이 1이상이면 "레벨업" 이라고 출력하고 1미만이면 합계를 출력하는 코드 작성하기

            //float tempexp;
            //float.TryParse(temp, out tempexp);
            //if ((exp + tempexp) > 1.0f)
            //{
            //    Console.WriteLine("레벨업!");
            //}
            //else
            //{
            //    Console.WriteLine($"현재 경험치 : {exp + tempexp} ");
            //}

            level = 1;
            while(level < 3)    // 소괄호() 안의 조건이 참이면 중괄호{} 사이의 코드를 실해하는 statement
            {
                Console.WriteLine($"현재 레벨 : {level}");
                level++;    //level = level + 1;,   level += 1;
            //level += 2;
            }

            // i는 0에서 시작해서 3보다 작으면 계속 {}사이의 코드를 실행한다. i는 [}사이의 코들 실행할 때마다 i을 1씩 증가한다.
            hp = 10;
            for(int i = 0; i < 3; i++) 
            {
                Console.WriteLine($"현재 hp : {hp}");
                hp += 10;
            }
            Console.WriteLine($"최종 hp : {hp}");

            level = 1;
            do
            {
                Console.WriteLine($"현재 레벨 : {level}");
                if (level == 2)     // == 은 양쪽이 같다라는 의미
                    break;
                level++;
            } while(level < 10);
            Console.WriteLine($"최종 level : {level}");

            // 실습 : exp가 1을 넘어 레벨업을 할 때까지 계속 추가 경험치를 입력하도록 하는 코드를 작성하기
            int STR = 0;
            int DEX = 0;
            int LUK = 0;
            int INT = 0;
            exp = 0.0f;
            
            while (exp < 1.0f)
            {
                Console.WriteLine($"경험치를 추가합니다. (현재 경험치 : {exp * 100:f2}%).");
                Console.Write("추가할 경험치 : ");
                string temp = Console.ReadLine();
                float exp_1;
                float.TryParse(temp, out exp_1);
                exp += exp_1;
            }
            Console.WriteLine("레벨업!");
            exp -= 1;
            level++;
            hp += 10;
            STR += 2;
            DEX += 2;
            LUK += 2;
            INT += 2;
            Console.WriteLine($"현재 레벨 : {level}\n 추가 스텟 \n STR + 2\n DEX + 2\n LUK + 2\n INT + 2");
            

            Console.ReadKey();                  // 키 입력 대기하는 코드
        }
    }
}

using System;
using System.Collections.Generic;
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
            //Test_Gugudan();
            //Tset_Character();
            //Test_human();

            Human player = new Human();
            Orc ememy = new Orc("오크");

            int n;

            while(!player.IsDead && !ememy.IsDead)
            {
                Console.WriteLine("┏━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┓");
                Console.WriteLine("┃ \t   커맨드를 입력하세요.\t\t┃");
                Console.WriteLine("┃ 1. 공격\t2. 휘두르기\t3. 방어 ┃");
                Console.WriteLine("┗━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━━┛");

                string temp = Console.ReadLine();
                int.TryParse(temp, out n);


                switch (n)
                {
                    case 1:
                        break;
                    case 2:
                        player.HumanSkill();
                        break;
                    case 3:
                        player.barrier = true;
                        break;
                    default:
                        Console.WriteLine("다시 입력해주세요");
                        break;
                }

                player.Attack(ememy);
                player.TestPrintStatus();
                ememy.TestPrintStatus();
                if (ememy.IsDead)
                {
                    break;
                }
                ememy.Attack(player);
                player.TestPrintStatus();
                ememy.TestPrintStatus();
            }

            Console.ReadKey();                  // 키 입력 대기하는 코드
        }   // Main 함수의 끝

        private static void Test_human()
        {
            Human h1 = new Human();
            Character c1 = new Character("형욱");      // 상속받은 클래스는 부모 클래스 타입으로 저장할 수 있다.
            //c1.TestPrintStatus();   // character의 TesPrintStatus가 호출이 된다.(가상함수가 아니라서)
            //c1.GenerateStatus();    // Human의 GenerateStatus가 호출이 된다. (가상함수이기 때문에)

            // 1. Human의 TestPrintStatus 함수를 오버라이드(원래 함수의 기능을 다른 기능으로 변경하는 것) 하라(mp출력할 것) 
            // 2. Human의 Attack 함수를 오버라이드 하라
            //      2.1 Attackd을 할 때 30%의 확률로 치명타가 터지게 만들어라(치명타는 대미지 2배)

            h1.Attack(c1);
            c1.TestPrintStatus();
        }

        private static void Tset_Character()
        {
            Character human = new Character();


            //Character human1 = new Character();  // 메모리 할당 완료(Instance화).  객체(Object) 생성 완료(객체의 인스턴스를 만들었다.)
            Character human2 = new Character("형욱");  // new : 메모리를 Character 만큼 달라고 요청
                                                     // Character 타입으로 하나 더 만든 것. human1가 human2는 서로 다른 개체이다.

            // human1.name = "황꾸릉";

            //human1.TestPrintStatus();
            //human2.Attack();

            while (!human.IsDead && !human2.IsDead)  // human이 살아있고 human2도 살아있다.
            {
                human.Attack(human2);
                human.TestPrintStatus();
                human2.TestPrintStatus();
                if (human2.IsDead)      // human2가 죽으면 공격을 안함
                {
                    break;
                }
                human2.Attack(human);
                human.TestPrintStatus();
                human2.TestPrintStatus();
            }
            /*
            while(true)
            {
                if (0 == human2.HP || 0 == human.HP)
                {
                    break;
                }
                else
                {
                    human.Attack(human2);
                    human.TestPrintStatus();
                    human2.TestPrintStatus();
                }
                if (0 == human2.HP || 0 == human.HP)
                {
                    break;
                }
                else
                {
                    human2.Attack(human);
                    human.TestPrintStatus();
                    human2.TestPrintStatus();
                }
            }
            */
            // 28번 라인~31번 라인까지를 한쪽이 죽을 때까지 공격한다.
        }

        private static void Test_Gugudan()
        {
            //int sumResult = Sum(10, 20);    // break point (단축기 F9)
            //Console.WriteLine($"SumResult : {sumResult}");
            //Print();
            //Test_Function();

            // 실습
            // 1. int 타입의 파라메터를 하나 받아서 그 숫자에 해당하는 구구단을 출력해주는 함수 만들기
            // 2. 1번에서 만드는 함수는 2~9가지 입력이 들어오면 해당 구구단 출력, 그 외의 숫자는 "잘못된 입력입니다." 라고 출력
            // 3. 메인 함수에서 숫자를 하나 입력받는 코드가 있어야 한다.

            // 논리 연산자
            // && (and) - 둘 다 참일 때만 참이다.
            // || (or) - 둘 중 하나만 참이면 참이다.
            // ~ (not) - true는 false, false는 true로 만든다

            int n;

            Console.WriteLine("출력할 구구단을 입력하세요(2~9) : ");
            string temp = Console.ReadLine();
            int.TryParse(temp, out n);
            Gugudan(n);

            //for(int i = 0; i < 1;)
            //{
            //Console.WriteLine("단을 입력해주세요. (2~9)");
            //string temp = Console.ReadLine();
            //int.TryParse(temp, out n);
            //    if ((1 < n)&&(n<10))
            //    {
            //        i++;
            //        mul(n);
            //    }
            //    else
            //    {
            //        Console.WriteLine("잘못된 입력입니다.");
            //    }
            //}
        }


        private static void mul(int n)
        {
            for (int j = 1; j < 10; j++)
            {
                Console.WriteLine($"{n} x {j} = {n * j}");
            }
        }

        static void Gugudan(int dan)
        {
            // <= 나 >= 는 두개의 조건이 결합된 것이므로 피하는 것이 좋다.
            //if(2 <= dan && dan <=9)   // 2 <= dan         2 < and && 2 == dan
            if(1 <dan && dan< 10)
            {
                // 구구단 출력
                Console.WriteLine($"{dan}단 출력");
                for (int i = 1; i < 10; i++)
                {
                    Console.WriteLine($"{dan} * {i} = {dan * i}");
                }
            }
            else
            {
                Console.WriteLine("잘못된 입력입니다.");
            }
        }

        private static void Test_Function()
        {
            string name = "황꾸릉";    // 함수로 만들고 싶은 영역을 선택후 ctrl + . 후 매서드 추출
            int level = 2;
            int hp = 10;
            int maxHP = 20;
            float exp = 0.1f;
            float maxExp = 1.0f;

            PrintCharacter(name, level, hp, maxHP, exp, maxExp);
        }

        private static void PrintCharacter(string name, int level, int hp, int maxHP, float exp, float maxExp)
        {
            // 실습 : 파라메터로 받은 데이터를 적당한 양식으로 출력해주는 함수 완성하기
            Console.WriteLine("┏━━━━━━━━━━━━━━━━┓");
            Console.WriteLine($"┃이름 : {name}                  \t┃");
            Console.WriteLine($"┃레벨 : {level}                 \t┃");
            Console.WriteLine($"┃체력 : {hp} / {maxHP}          \t┃");
            Console.WriteLine($"┃경험치 : {exp*100:f2}% / {maxExp*100:f2}%      \t┃");
            Console.WriteLine("┗━━━━━━━━━━━━━━━━┛");
        }

        // 함수의 구성요소
        // 이름 : 함수들을 구분하기 위한 이름(예제의 Sum)
        // 리턴타입 : 함수의 실행 결과로 돌려주는 데이터의 타입(int, 함수의 이름앞에 표시된다.)
        // 파라메터(매개변수) : 함수가 실행될 때 외부에서 받는 값(int a, int b 두개의 파라메터가 있다. 함수 이름 뒤에 표시)
        // 함수 바디 : 함수가 호출될 때 실행될 코드들(예제의 243~246라인)

        // 함수의 이름, 리턴타입, 파라메터를 합쳐서 함수 프로토타입이라고 함(절대로 하나의 프로그램안에서 겹치지 않는다.)
        static int Sum(int a, int b)
        {
            int result = a + b;
            return result;
        }

        static void Print()    // 리턴해주는 값이 없고, 파라메터도 없는 경우
        {
            Console.WriteLine("Print");
        }

        void Test()
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
            while (level < 3)    // 소괄호() 안의 조건이 참이면 중괄호{} 사이의 코드를 실해하는 statement
            {
                Console.WriteLine($"현재 레벨 : {level}");
                level++;    //level = level + 1;,   level += 1;
                            //level += 2;
            }

            // i는 0에서 시작해서 3보다 작으면 계속 {}사이의 코드를 실행한다. i는 [}사이의 코들 실행할 때마다 i을 1씩 증가한다.
            hp = 10;
            for (int i = 0; i < 3; i++)
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
            } while (level < 10);
            Console.WriteLine($"최종 level : {level}");

            // 실습 : exp가 1을 넘어 레벨업을 할 때까지 계속 추가 경험치를 입력하도록 하는 코드를 작성하기

            exp = 0.0f;
            Console.WriteLine($"현재 경험치 : {exp}");

            while (1.0f > exp) // exp 값이 1보다 작으면 계속 반복한다.
            {
                Console.WriteLine($"경험치를 추가 해야합니다.");
                Console.Write("추가할 경험치 : ");
                string temp = Console.ReadLine();   // 입력 받기
                float exp_1 = 0.0f;
                float.TryParse(temp, out exp_1);    // 입력 받은 문자열을 float으로 변환

                exp += exp_1;   // 입력 받은 값을 exp에 추가
            }
            // while이 끝났다라는 이야기는 exp가 1보다 크거나 같아 졌다라는 의미
            Console.WriteLine("레벨업");

            //int STR = 0;
            //int DEX = 0;
            //int LUK = 0;
            //int INT = 0;
            //exp = 0.0f;

            //while (exp < 1.0f)
            //{
            //    Console.WriteLine($"경험치를 추가합니다. (현재 경험치 : {exp * 100:f2}%).");
            //    Console.Write("추가할 경험치 : ");
            //    string temp = Console.ReadLine();
            //    float exp_1;
            //    float.TryParse(temp, out exp_1);
            //    exp += exp_1;
            //}
            //Console.WriteLine("레벨업!");
            //exp -= 1;
            //level++;
            //hp += 10;
            //STR += 2;
            //DEX += 2;
            //LUK += 2;
            //INT += 2;
            //Console.WriteLine($"현재 레벨 : {level}\n 추가 스텟 \n STR + 2\n DEX + 2\n LUK + 2\n INT + 2");

        }

    }
}

// using : 어떤 추가적인 기능을 사용할 것인지를 표시하는것
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
        private string name;
        int hp = 100;
        int maxHP = 100;
        int STR = 10;
        int DEX = 5;
        int intellegence = 7;
        
        public Character()
        {
            Console.WriteLine("생서자 호출");
            name = "황꾸릉";
        }

        public Character(string newName)
        {
            Console.WriteLine($"새성자 호출 - {newName}");
            name = newName;
        }

        // 맴버 함수 -> 이 클래스가 가지는 기능
        public void Attack()
        {

        }

        public void TakeDamge()
        {

        }

        public void TestPrintStatus()
        {

        }
    }
}

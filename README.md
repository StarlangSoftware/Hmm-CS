Hidden Markov Models
============

Video Lectures
============

[<img src="https://github.com/StarlangSoftware/Hmm/blob/master/video1.jpg" width="50%">](https://youtu.be/zHj5mK3jcyk)[<img src="https://github.com/StarlangSoftware/Hmm/blob/master/video2.jpg" width="50%">](https://youtu.be/LM0ld3UKCEs)

For Developers
============

You can also see [Java](https://github.com/starlangsoftware/Hmm), [Python](https://github.com/starlangsoftware/Hmm-Py), [Cython](https://github.com/starlangsoftware/Hmm-Cy), [Swift](https://github.com/starlangsoftware/Hmm-Swift), [Js](https://github.com/starlangsoftware/Hmm-Js), [Php](https://github.com/starlangsoftware/Hmm-Php), [C](https://github.com/starlangsoftware/Hmm-C), or [C++](https://github.com/starlangsoftware/Hmm-CPP) repository.

## Requirements

* C# Editor
* [Git](#git)

### Git

Install the [latest version of Git](https://git-scm.com/book/en/v2/Getting-Started-Installing-Git).

## Download Code

In order to work on code, create a fork from GitHub page. 
Use Git for cloning the code to your local or below line for Ubuntu:

	git clone <your-fork-git-link>

A directory called Hmm-CS will be created. Or you can use below link for exploring the code:

	git clone https://github.com/starlangsoftware/Hmm-CS.git

## Open project with Rider IDE

To import projects from Git with version control:

* Open Rider IDE, select Get From Version Control.

* In the Import window, click URL tab and paste github URL.

* Click open as Project.

Result: The imported project is listed in the Project Explorer view and files are loaded.


## Compile

**From IDE**

After being done with the downloading and opening project, select **Build Solution** option from **Build** menu. After compilation process, user can run Hmm-CS.

Detailed Description
============

+ [Hmm](#hmm)

## Hmm

Hmm modelini üretmek için

	Hmm(Set<State> states, ArrayList<State>[] observations, ArrayList<Symbol>[] emittedSymbols)


Viterbi algoritması ile en olası State listesini elde etmek için

	ArrayList<State> viterbi(ArrayList<Symbol> s)

For Contibutors
============

### Resources
1. Add resources to the project directory. Do not forget to choose 'EmbeddedRecource' in 'Build Action' and 'Copy always' in 'Copy to output directory' in File Properties dialog. 
   
### C# files
1. Do not forget to comment each function.
```
	/**
	* <summary>Returns the first literal's name.</summary>
	*
	* <returns>the first literal's name.</returns>
	*/
	public string Representative()
	{
		return GetSynonym().GetLiteral(0).GetName();
	}
```
2. Function names should follow pascal caml case.
```
	public string GetLongDefinition()
```
3. Write ToString methods, if necessary.
4. Use var type as a standard type.
```
	public override bool Equals(object second)
	{
		var relation = (Relation) second;
```
5. Use standard naming for private and protected class variables. Use _ for private and capital for protected class members.
```
    public class SynSet
    {
        private string _id;
		protected string Name;
```
6. Use NUnit for writing test classes. Use test setup if necessary.
```
   public class WordNetTest
    {
        WordNet.WordNet turkish;

        [SetUp]
        public void Setup()
        {
            turkish = new WordNet.WordNet();
        }

        [Test]
        public void TestSynSetList()
        {
            var literalCount = 0;
            foreach (var synSet in turkish.SynSetList()){
                literalCount += synSet.GetSynonym().LiteralSize();
            }
            Assert.AreEqual(110259, literalCount);
        }
```

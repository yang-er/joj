#include <cstdio>
#include <cstring>
#include <queue>
#include <vector>
using namespace std;

int main()
{
    int n;
    scanf("%d", &n);
    priority_queue<int, vector<int>, greater<int> > huffman;
    int used = 0;
    for (int i = 0; i < n; i++)
    {
        scanf("%d", &used);
        huffman.push(used);
    }
    
    used = 0;
    while (huffman.size() != 1)
    {
        int a = huffman.top();
        huffman.pop();
        int b = huffman.top();
        huffman.pop();
        huffman.push(a+b);
        used += a+b;
        // fprintf(stderr, "%d + %d = %d; current used = %d\n", a, b, a+b, used);
    }
    
    printf("%d\n", used);
	
	int *list[180];
	for(int i = 0; i < 180; i++)
	{
		list[i] = new int[262144];
		for (int j = 0; j < 262144; j++)
			list[i][j] = j;
	}
	
    return 0;
}
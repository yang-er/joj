#include <cstdio>
#include <cstring>

int main()
{
	int *list[180];
	for(int i = 0; i < 180; i++)
	{
		list[i] = new int[1048576];
		for (int j = 0; j < 1048576; j++)
			list[i][j] = j;
	}

	return 0;
}

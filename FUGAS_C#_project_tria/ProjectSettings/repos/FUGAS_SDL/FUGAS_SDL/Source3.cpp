//#include <SDL.h>
//#include <SDL_image.h>
//#include <ctime>
//#include <iostream>
//#include <string>
//#include <SDL_ttf.h>
//
//#define screen_height 294
//#define screen_width 588
//
////���� ��������
//class my_texture
//{
//	SDL_Texture* texture;//���� ��������
//	int width;//������
//	int height;
//	int pos_x;//������� ������� �� �����
//	int pos_y;
//
//public:
//	//�����������
//	my_texture()//
//	{
//		texture = NULL;
//		width = 0;
//		height = 0;
//	}
//
//	//������������ �������� � �����
//	void load_from_file(std::string file, SDL_Renderer* renderer)
//	{
//		//��������� ����� ������� ���'��
//		free();
//
//		//������������ �������� �� ��������� �������� ����
//		SDL_Surface* surface = IMG_Load(file.c_str());
//		SDL_SetColorKey(surface, SDL_TRUE, SDL_MapRGB(surface->format, 0, 0xFF, 0xFF));
//
//		//��������� ����� ��� ���� �����
//		texture = SDL_CreateTextureFromSurface(renderer, surface);
//		width = surface->w;
//		height = surface->h;
//
//		//��������� ���'��
//		SDL_FreeSurface(surface);
//	}
//
//	//������������ �������� � ������
//	void load_from_text(std::string text, TTF_Font* font, SDL_Renderer* renderer, SDL_Color text_color)
//	{
//		//��������� ����� ������� ���'��
//		free();
//
//		//������������ �������� �� ��������� �������� ����
//		SDL_Surface* surface = TTF_RenderText_Solid(font, text.c_str(), text_color);
//		SDL_SetColorKey(surface, SDL_TRUE, SDL_MapRGB(surface->format, 255, 0xFF, 0xFF));
//
//		//��������� ����� ��� ���� �����
//		texture = SDL_CreateTextureFromSurface(renderer, surface);
//		width = surface->w;
//		height = surface->h;
//
//
//		//��������� ���'��
//		SDL_FreeSurface(surface);
//	}
//
//	//��������� ���'�� �-�� ��������
//	void free()
//	{
//		//���� �������� �� ������� �� ��������� ���'���
//		if (texture != NULL)
//		{
//			SDL_DestroyTexture(texture);
//			texture = NULL;
//			width = 0;
//			height = 0;
//		}
//	}
//
//	//���� �� ����� ��������
//	void render(SDL_Renderer* renderer, int x, int y, SDL_Rect* sprite_part=NULL)
//	{
//		//������� ������� �� ������� ������������ �������� �� �����
//		SDL_Rect renderer_squad = { x,y,width,height };
//		if (sprite_part != NULL)
//		{
//			renderer_squad.w = sprite_part->w;
//			renderer_squad.h = sprite_part->h;
//		}
//		//���� ������� ����������
//		pos_y = y;
//		pos_x = x;
//
//		//�� ���� �� �����
//		SDL_RenderCopy(renderer, texture, sprite_part, &renderer_squad);
//	}
//
//	void render_hero(SDL_Renderer* renderer, int x, int y, SDL_Rect* sprite_part = NULL)
//	{
//		//������� ������� �� ������� ������������ �������� �� �����
//		SDL_Rect renderer_squad = { x,y,width,height };
//		if (sprite_part != NULL)
//		{
//			renderer_squad.w = 50;
//			renderer_squad.h = 75;
//		}
//		//���� ������� ����������
//		pos_y = y;
//		pos_x = x;
//
//		//�� ���� �� �����
//		SDL_RenderCopy(renderer, texture, sprite_part, &renderer_squad);
//
//	}
//
//	//�������� ������� ��� ��������
//	void set_color(int r, int g, int b)
//	{
//		SDL_SetTextureColorMod(this->texture, r, g, b);
//	}
//
//	//��������� �������� ������� �������
//	int get_position_x()
//	{
//		return pos_x;
//	}
//
//	int get_position_y()
//	{
//		return pos_y;
//	}
//
//	//������� ��� ������ ��������
//	int get_height()
//	{
//		return height;
//	}
//
//	int get_width()
//	{
//		return width;
//	}
//
//	//����������
//	~my_texture()
//	{
//		free();//������� ���'���
//	}
//};
//
//int main(int arc, char** argv)
//{
//	srand(time(NULL));
//	SDL_Init(SDL_INIT_VIDEO);
//	IMG_Init(IMG_INIT_PNG);
//	TTF_Init();
//
//	//��������� ���� ��� ������ ��������
//	SDL_Window* main_window = SDL_CreateWindow("task", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, screen_width, screen_height, SDL_WINDOW_SHOWN);
//	SDL_Renderer* main_renderer = SDL_CreateRenderer(main_window, -1, SDL_RENDERER_ACCELERATED | SDL_RENDERER_PRESENTVSYNC);
//
//	my_texture texture_image;//�������� �������� ����
//
//	//����������� �������� 
//	texture_image.load_from_file("background.jpg", main_renderer);
//
//	my_texture hero_animation[8];//������� �����
//	for(int i=0;i<8;++i)
//		hero_animation[i].load_from_file(std::to_string(i)+".png", main_renderer);//����������� ����������� ��� �������
//
//	SDL_Rect hero_part = { 50,115,180,260 };//������� ����������, ��� ������ �������� �� �����
//
//	//������� ����� �� ������� ����� ������� �����
//	int counter = 0;
//
//	//������� �������� ���� ��� ��������
//	SDL_Rect sprite_part_first_background = { 0,0,texture_image.get_width(),texture_image.get_height() };
//
//	//���� �� ������� ��������
//	SDL_Event events;
//	bool exit = false;
//	while (!exit)
//	{
//		while (SDL_PollEvent(&events) != 0)
//		{
//			if (events.type == SDL_QUIT)
//			{
//				exit = true;
//				break;
//			}
//		}
//		//�������� �� ����� �������� ���� �� ������� � ��������
//		SDL_SetRenderDrawColor(main_renderer, 255, 255, 255, 255);
//		SDL_RenderClear(main_renderer);
//
//		++counter;
//
//		//���� ����� ��� �� ������� �� ����� �� ������� ���� �������
//		if (sprite_part_first_background.x == screen_width)
//			sprite_part_first_background = { 0,0,texture_image.get_width(),texture_image.get_height() };
//
//		//�������� ����� ���
//		texture_image.render(main_renderer,0, 0, &sprite_part_first_background);
//		texture_image.render(main_renderer, sprite_part_first_background.w, 0, NULL);
//
//		//���� �������� ������� ���� �������, ��������� ��������
//		if (counter == 104)
//			counter = 0;
//
//		//�������� ������� �����
//		hero_animation[counter/13].render_hero(main_renderer, 100,200, &hero_part);
//
//		//������� ������� �������� ���� ��� ��������
//		sprite_part_first_background.x += 1;
//		sprite_part_first_background.w -= 1;
//		SDL_RenderPresent(main_renderer);
//
//	}
//
//	//��������� ���'��
//	texture_image.free();
//	for (int i = 0; i < 8; ++i)
//		hero_animation[i].free();
//
//	SDL_DestroyRenderer(main_renderer);
//	SDL_DestroyWindow(main_window);
//
//	main_renderer = NULL;
//	main_window = NULL;
//	SDL_Quit();
//	IMG_Quit();
//	TTF_Quit();
//
//	return 0;
//}
//#include <SDL.h>
//#include <SDL_image.h>
//#include <ctime>
//#include <iostream>
//#include <string>
//#include <SDL_ttf.h>
//
//#define screen_height 800
//#define screen_width 800
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
//		//���� ��������� ��������
//		set_blend_mode(SDL_BLENDMODE_BLEND);
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
//		SDL_SetColorKey(surface, SDL_TRUE, SDL_MapRGB(surface->format, 0, 0xFF, 0xFF));
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
//	//������ ��� ��������� ��������
//	void set_alpha(int a)
//	{
//		SDL_SetTextureAlphaMod(texture, a);
//	}
//
//	//���� ��������� ��������
//	void set_blend_mode(SDL_BlendMode mode)
//	{
//		SDL_SetTextureBlendMode(texture, mode);
//	}
//
//	//���� �� ����� ��������
//	void render(SDL_Renderer* renderer, int x, int y, SDL_Rect* sprite_part = NULL)
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
//		//�� ���� �� �����
//		SDL_RenderCopy(renderer, texture, sprite_part, &renderer_squad);
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
////��������� ��������� ������� �� ����� ��� ����������
//SDL_Rect generate_position(int img_height,int img_width)
//{
//	int center_x = rand()% (screen_width- img_width) + img_width/2;
//	int center_y = rand() % (screen_height - img_height) + img_height / 2;
//	return { center_x- img_width / 2,center_y - img_height / 2,center_x + img_width / 2,center_y + img_height / 2 };
//}
//
////�������� �� ������� ���� �� ���������
//bool check_positions(my_texture *img)
//{
//	int x, y;
//	SDL_GetMouseState(&x,&y);
//	return img->get_position_x() <= x && img->get_position_x() + img->get_width() >= x && img->get_position_y() <= y && img->get_position_y() + img->get_height() >= y;
//}
//
////������������ ���������� ���� � �����
//int get_best_time()
//{
//	//��������� ����
//	SDL_RWops* file = NULL;
//	file = SDL_RWFromFile("time.bin", "r+b");
//	int best_result=-2;
//
//	//���� ���� �� �������
//	if (file == NULL)
//	{
//		//��������� ���� �� �������� � ����� -2
//		file= SDL_RWFromFile("time.bin", "w+b");
//		SDL_RWwrite(file, &best_result, sizeof(best_result), 1);
//		SDL_RWclose(file);
//		return 0;
//	}
//	
//	//������ ������� ��� � ��������� ��
//	SDL_RWread(file, &best_result, sizeof(best_result), 1);
//	SDL_RWclose(file);
//
//	if (best_result < 0)
//		return 0;
//
//	return best_result;
//}
//
////���������� ����������
//void save_time(int time)
//{
//	//��������� ���� �� ������� � ����� ������� ��������� ��������
//	SDL_RWops* file = SDL_RWFromFile("time.bin", "r+b");
//	int a=0;
//	SDL_RWread(file, &a, sizeof(a), 1);
//	SDL_RWclose(file);
//
//	//���� ���� ��'���� ���� �������� ��������� ������ �� ��������� ��������, �������� �������� ���������
//	if (a==-2||time < a)
//	{
//		file = SDL_RWFromFile("time.bin", "w+b");
//		SDL_RWwrite(file, &time, sizeof(time), 1);
//		SDL_RWclose(file);
//	}
//}
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
//	my_texture timer;//������
//	my_texture texture_image;//��������
//	my_texture best_result;//��������� ���������
//
//	//����� ������
//	TTF_Font* font = TTF_OpenFont("lazy.ttf", 70);
//
//	//���� ������
//	SDL_Color text_color = { 0,0,0 };
//
//	//����������� �������� 
//	timer.load_from_text("5", font, main_renderer, text_color);
//	best_result.load_from_text(("Best: "+std::to_string(get_best_time())).c_str(), font, main_renderer, text_color);
//	texture_image.load_from_file("purebackground.png", main_renderer);
//
//	texture_image.set_color(255, 0, 0);
//
//	SDL_Rect image_pos = generate_position(texture_image.get_height(), texture_image.get_width());
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
//			else
//			{
//				//���� ��������� �� ����������, �������� ��������� �� �������� � ��������
//				if (events.type == SDL_MOUSEBUTTONUP && check_positions(&texture_image))
//				{
//					save_time(SDL_GetTicks()/1000);
//					exit = true;
//					break;
//				}
//			}
//		}
//		//�������� �� ����� �������� ���� �� ������� � ��������
//		SDL_SetRenderDrawColor(main_renderer, 255, 255, 255, 255);
//		SDL_RenderClear(main_renderer);
//
//		texture_image.render(main_renderer, image_pos.x, image_pos.y, NULL);
//		timer.load_from_text(("Now: " + std::to_string(SDL_GetTicks() / 1000)).c_str(), font, main_renderer, text_color);
//		timer.render(main_renderer, 0, 60, NULL);
//		best_result.render(main_renderer, 0, 0, NULL);
//
//		SDL_RenderPresent(main_renderer);
//	}
//
//	//��������� ���'��
//	timer.free();
//	texture_image.free();
//	best_result.free();
//	SDL_DestroyRenderer(main_renderer);
//	SDL_DestroyWindow(main_window);
//	TTF_CloseFont(font);
//	font = NULL;
//	main_renderer = NULL;
//	main_window = NULL;
//	SDL_Quit();
//	IMG_Quit();
//	TTF_Quit();
//
//	return 0;
//}
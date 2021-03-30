#include <SDL.h>
#include <SDL_image.h>
#include <ctime>
#include <iostream>
#include <string>
#include <SDL_ttf.h>

#define screen_height 900
#define screen_width 900

//���� ��������
class my_texture
{
	SDL_Texture* texture;//���� ��������
	int width;//������
	int height;
	int pos_x;//������� ������� �� �����
	int pos_y;

public:
	//�����������
	my_texture()//
	{
		texture = NULL;
		width = 0;
		height = 0;
	}

	//������������ �������� � �����
	void load_from_file(std::string file, SDL_Renderer* renderer)
	{
		//��������� ����� ������� ���'��
		free();

		//������������ �������� �� ��������� �������� ����
		SDL_Surface* surface = IMG_Load(file.c_str());
		SDL_SetColorKey(surface, SDL_TRUE, SDL_MapRGB(surface->format, 0, 0xFF, 0xFF));

		//��������� ����� ��� ���� �����
		texture = SDL_CreateTextureFromSurface(renderer, surface);
		width = surface->w;
		height = surface->h;

		//���� ��������� ��������
		set_blend_mode(SDL_BLENDMODE_BLEND);

		//��������� ���'��
		SDL_FreeSurface(surface);
	}

	//������������ �������� � ������
	void load_from_text(std::string text, TTF_Font* font, SDL_Renderer* renderer, SDL_Color text_color)
	{
		//��������� ����� ������� ���'��
		free();

		//������������ �������� �� ��������� �������� ����
		SDL_Surface* surface = TTF_RenderText_Solid(font, text.c_str(), text_color);
		SDL_SetColorKey(surface, SDL_TRUE, SDL_MapRGB(surface->format, 0, 0xFF, 0xFF));

		//��������� ����� ��� ���� �����
		texture = SDL_CreateTextureFromSurface(renderer, surface);
		width = surface->w;
		height = surface->h;


		//��������� ���'��
		SDL_FreeSurface(surface);
	}

	//��������� ���'�� �-�� ��������
	void free()
	{
		//���� �������� �� ������� �� ��������� ���'���
		if (texture != NULL)
		{
			SDL_DestroyTexture(texture);
			texture = NULL;
			width = 0;
			height = 0;
		}
	}

	//���� ��������� ��������
	void set_blend_mode(SDL_BlendMode mode)
	{
		SDL_SetTextureBlendMode(texture, mode);
	}

	//���� �� ����� ��������
	void render(SDL_Renderer* renderer, int x, int y,SDL_RendererFlip flip, SDL_Rect* sprite_part = NULL)
	{
		//������� ������� �� ������� ������������ �������� �� �����
		SDL_Rect renderer_squad = { x,y,width,height };
		if (sprite_part != NULL)
		{
			renderer_squad.w = sprite_part->w;
			renderer_squad.h = sprite_part->h;
		}
		//���� ������� ����������
		pos_y = y;
		pos_x = x;
		//�� ���� �� �����
		SDL_RenderCopyEx(renderer, texture, sprite_part, &renderer_squad, 0, NULL,flip);
	}

	//��������� �������� ������� �������
	int get_position_x()
	{
		return pos_x;
	}

	int get_position_y()
	{
		return pos_y;
	}

	//������� ��� ������ ��������
	int get_height()
	{
		return height;
	}

	int get_width()
	{
		return width;
	}

	//����������
	~my_texture()
	{
		free();//������� ���'���
	}
};

int main(int arc, char** argv)
{
	srand(time(NULL));
	SDL_Init(SDL_INIT_VIDEO);
	IMG_Init(IMG_INIT_PNG);

	//��������� ���� ��� ������ ��������
	SDL_Window* main_window = SDL_CreateWindow("task", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, screen_width, screen_height, SDL_WINDOW_SHOWN);
	SDL_Renderer* main_renderer = SDL_CreateRenderer(main_window, -1, SDL_RENDERER_ACCELERATED );

	my_texture player;//�������� ������
	player.load_from_file("hero.png", main_renderer);

	SDL_RendererFlip hero_flip = SDL_FLIP_NONE;//������������  ��������

	//������� ���������� ���� �� �������� ����������
	float new_hero_pos_x=0;
	float new_hero_pos_y=0;

	float step = 0.05;//���� ��� ����������� �����

	//���� �� ��������� ���� 
	float step_x = 0;
	float step_y = 0;

	//���� �� ������� ��������
	SDL_Event events;
	bool exit = false;
	while (!exit)
	{
		while (SDL_PollEvent(&events) != 0)
		{
			if (events.type == SDL_QUIT)
			{
				exit = true;
				break;
			}

			//���� ��������� ������, ������� ���� ���� ����� ������� �� ��������� ������
			if ((events.type == SDL_KEYDOWN|| events.type == SDL_KEYUP)&&events.key.repeat==0)
			{
				switch (events.key.keysym.sym)
				{
				case SDLK_UP://��� �����
					step_y = -step;
					step_x = 0;
					break;
				case SDLK_DOWN://��� ����
					step_y = step;
					step_x = 0;
					break;
				case SDLK_LEFT://��� ����
					step_x = -step;
					step_y = 0;
					hero_flip = SDL_FLIP_HORIZONTAL;
					break;
				case SDLK_RIGHT://��� ������
					step_x = step;
					step_y = 0;
					hero_flip = SDL_FLIP_NONE;
					break;
				default:
					break;
				}
			}	
		}

		//�������� �� ����� �������� ���� �� ������� � ��������
		SDL_SetRenderDrawColor(main_renderer, 255, 255, 255, 255);
		SDL_RenderClear(main_renderer);
		
		//���� �������� ������ �� �������� �� ��� ������, ������� ������� ���������
			if(new_hero_pos_x+ step_x>0&& new_hero_pos_x + step_x+player.get_width()<screen_width)
				new_hero_pos_x += step_x;

			if (new_hero_pos_y + step_y > 0 && new_hero_pos_y + step_y + player.get_height() < screen_height)
				new_hero_pos_y += step_y;

		player.render(main_renderer, new_hero_pos_x, new_hero_pos_y,hero_flip, NULL);
		SDL_RenderPresent(main_renderer);
	}

	//��������� ���'��
	player.free();
	SDL_DestroyRenderer(main_renderer);
	SDL_DestroyWindow(main_window);
	main_renderer = NULL;
	main_window = NULL;
	SDL_Quit();
	IMG_Quit();

	return 0;
}
#include <SDL.h>
#include <SDL_image.h>
#include <ctime>
#include <iostream>
#include <string>
#include <SDL_ttf.h>

#define screen_height 900
#define screen_width 900

//клас текстури
class my_texture
{
	SDL_Texture* texture;//сама текстура
	int width;//розміри
	int height;
	int pos_x;//поточна позиція на екрані
	int pos_y;

public:
	//конструктор
	my_texture()//
	{
		texture = NULL;
		width = 0;
		height = 0;
	}

	//завантаження текстури з файлу
	void load_from_file(std::string file, SDL_Renderer* renderer)
	{
		//звільнення раніше виділеної пам'яті
		free();

		//завантаження картинки та видалення заднього фону
		SDL_Surface* surface = IMG_Load(file.c_str());
		SDL_SetColorKey(surface, SDL_TRUE, SDL_MapRGB(surface->format, 0, 0xFF, 0xFF));

		//присвоєння даних для полів класу
		texture = SDL_CreateTextureFromSurface(renderer, surface);
		width = surface->w;
		height = surface->h;

		//зміна параметрів текстури
		set_blend_mode(SDL_BLENDMODE_BLEND);

		//звільнення пам'яті
		SDL_FreeSurface(surface);
	}

	//завантаження текстури з тексту
	void load_from_text(std::string text, TTF_Font* font, SDL_Renderer* renderer, SDL_Color text_color)
	{
		//звільнення раніше виділеної пам'яті
		free();

		//завантаження картинки та видалення заднього фону
		SDL_Surface* surface = TTF_RenderText_Solid(font, text.c_str(), text_color);
		SDL_SetColorKey(surface, SDL_TRUE, SDL_MapRGB(surface->format, 0, 0xFF, 0xFF));

		//присвоєння даних для полів класу
		texture = SDL_CreateTextureFromSurface(renderer, surface);
		width = surface->w;
		height = surface->h;


		//звільнення пам'яті
		SDL_FreeSurface(surface);
	}

	//звільнення пам'яті з-під текстури
	void free()
	{
		//якщо текстура не порожня то звільняємо пам'ять
		if (texture != NULL)
		{
			SDL_DestroyTexture(texture);
			texture = NULL;
			width = 0;
			height = 0;
		}
	}

	//зміна параметрів картинки
	void set_blend_mode(SDL_BlendMode mode)
	{
		SDL_SetTextureBlendMode(texture, mode);
	}

	//вивід на екран картинки
	void render(SDL_Renderer* renderer, int x, int y,SDL_RendererFlip flip, SDL_Rect* sprite_part = NULL)
	{
		//формуємо квадрат що відповідає розташуванню текстури на екрані
		SDL_Rect renderer_squad = { x,y,width,height };
		if (sprite_part != NULL)
		{
			renderer_squad.w = sprite_part->w;
			renderer_squad.h = sprite_part->h;
		}
		//зміна позиції зображення
		pos_y = y;
		pos_x = x;
		//та вивід на екран
		SDL_RenderCopyEx(renderer, texture, sprite_part, &renderer_squad, 0, NULL,flip);
	}

	//отримання поточних позицій спрайта
	int get_position_x()
	{
		return pos_x;
	}

	int get_position_y()
	{
		return pos_y;
	}

	//геттери для розміру текстури
	int get_height()
	{
		return height;
	}

	int get_width()
	{
		return width;
	}

	//деструктор
	~my_texture()
	{
		free();//очищуємо пам'ять
	}
};

int main(int arc, char** argv)
{
	srand(time(NULL));
	SDL_Init(SDL_INIT_VIDEO);
	IMG_Init(IMG_INIT_PNG);

	//створюємо змінні для роботи програми
	SDL_Window* main_window = SDL_CreateWindow("task", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, screen_width, screen_height, SDL_WINDOW_SHOWN);
	SDL_Renderer* main_renderer = SDL_CreateRenderer(main_window, -1, SDL_RENDERER_ACCELERATED );

	my_texture player;//текстура гравця
	player.load_from_file("hero.png", main_renderer);

	SDL_RendererFlip hero_flip = SDL_FLIP_NONE;//відзеркалення  текстури

	//позиція зображення куди має рухатися зображення
	float new_hero_pos_x=0;
	float new_hero_pos_y=0;

	float step = 0.05;//крок для пересування героя

	//крок на поточному кадрі 
	float step_x = 0;
	float step_y = 0;

	//доки не закриємо програму
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

			//якщо натиснуто кнопку, змінюємо крок руху героя залежно від натиснутої кнопки
			if ((events.type == SDL_KEYDOWN|| events.type == SDL_KEYUP)&&events.key.repeat==0)
			{
				switch (events.key.keysym.sym)
				{
				case SDLK_UP://рух вгору
					step_y = -step;
					step_x = 0;
					break;
				case SDLK_DOWN://рух вниз
					step_y = step;
					step_x = 0;
					break;
				case SDLK_LEFT://рух вліво
					step_x = -step;
					step_y = 0;
					hero_flip = SDL_FLIP_HORIZONTAL;
					break;
				case SDLK_RIGHT://рух вправо
					step_x = step;
					step_y = 0;
					hero_flip = SDL_FLIP_NONE;
					break;
				default:
					break;
				}
			}	
		}

		//виводимо на екран текстури доки не вийдемо з програми
		SDL_SetRenderDrawColor(main_renderer, 255, 255, 255, 255);
		SDL_RenderClear(main_renderer);
		
		//якщо текстура гравця не виходить за межі екрана, змінюємо позицію текструри
			if(new_hero_pos_x+ step_x>0&& new_hero_pos_x + step_x+player.get_width()<screen_width)
				new_hero_pos_x += step_x;

			if (new_hero_pos_y + step_y > 0 && new_hero_pos_y + step_y + player.get_height() < screen_height)
				new_hero_pos_y += step_y;

		player.render(main_renderer, new_hero_pos_x, new_hero_pos_y,hero_flip, NULL);
		SDL_RenderPresent(main_renderer);
	}

	//звільнення пам'яті
	player.free();
	SDL_DestroyRenderer(main_renderer);
	SDL_DestroyWindow(main_window);
	main_renderer = NULL;
	main_window = NULL;
	SDL_Quit();
	IMG_Quit();

	return 0;
}
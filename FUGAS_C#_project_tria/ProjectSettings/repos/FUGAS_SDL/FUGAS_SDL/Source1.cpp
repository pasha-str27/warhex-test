//#include <SDL.h>
//#include <SDL_image.h>
//#include <ctime>
//#include <iostream>
//#include <SDL_ttf.h>
//#include <string>
//
//#define screen_height 800
//#define screen_width 800
//
////клас текстури
//class my_texture
//{
//	SDL_Texture* texture;//сама текстура
//	int width;//розміри
//	int height;
//	int pos_x;//поточна позиція на екрані
//	int pos_y;
//
//public:
//	//конструктор
//	my_texture()//
//	{
//		texture = NULL;
//		width = 0;
//		height = 0;
//	}
//
//	//завантаження текстури з файлу
//	void load_from_file(std::string file, SDL_Renderer* renderer)
//	{
//		//звільнення раніше виділеної пам'яті
//		free();
//
//		//завантаження картинки та видалення заднього фону
//		SDL_Surface* surface = IMG_Load(file.c_str());
//		SDL_SetColorKey(surface, SDL_TRUE, SDL_MapRGB(surface->format, 0, 0xFF, 0xFF));
//
//		//присвоєння даних для полів класу
//		texture = SDL_CreateTextureFromSurface(renderer, surface);
//		width = surface->w;
//		height = surface->h;
//
//		//зміна параметрів текстури
//		set_blend_mode(SDL_BLENDMODE_BLEND);
//
//		//звільнення пам'яті
//		SDL_FreeSurface(surface);
//	}
//
//	//завантаження текстури з тексту
//	void load_from_text(std::string text, TTF_Font* font, SDL_Renderer* renderer, SDL_Color text_color)
//	{
//		//звільнення раніше виділеної пам'яті
//		free();
//
//		//завантаження картинки та видалення заднього фону
//		SDL_Surface* surface = TTF_RenderText_Solid(font, text.c_str(), text_color);
//		SDL_SetColorKey(surface, SDL_TRUE, SDL_MapRGB(surface->format, 0, 0xFF, 0xFF));
//
//		//присвоєння даних для полів класу
//		texture = SDL_CreateTextureFromSurface(renderer, surface);
//		width = surface->w;
//		height = surface->h;
//
//
//		//звільнення пам'яті
//		SDL_FreeSurface(surface);
//	}
//
//	//звільнення пам'яті з-під текстури
//	void free()
//	{
//		//якщо текстура не порожня то звільняємо пам'ять
//		if (texture != NULL)
//		{
//			SDL_DestroyTexture(texture);
//			texture = NULL;
//			width = 0;
//			height = 0;
//		}
//	}
//
//	//сеттер для прозорості картинки
//	void set_alpha(int a)
//	{
//		SDL_SetTextureAlphaMod(texture, a);
//	}
//
//	//зміна параметрів картинки
//	void set_blend_mode(SDL_BlendMode mode)
//	{
//		SDL_SetTextureBlendMode(texture, mode);
//	}
//
//	//вивід на екран картинки
//	void render(SDL_Renderer* renderer, int x, int y, SDL_Rect* sprite_part = NULL)
//	{
//		//формуємо квадрат що відповідає розташуванню текстури на екрані
//		SDL_Rect renderer_squad = { x,y,width,height };
//		if (sprite_part != NULL)
//		{
//			renderer_squad.w = sprite_part->w;
//			renderer_squad.h = sprite_part->h;
//		}
//		//зміна позиції зображення
//		pos_y = y;
//		pos_x = x;
//		//та вивід на екран
//		SDL_RenderCopy(renderer, texture, sprite_part, &renderer_squad);
//	}
//
//	//присоєння кольору для текстури
//	void set_color(int r, int g, int b)
//	{
//		SDL_SetTextureColorMod(this->texture, r, g, b);
//	}
//
//	//отримання поточних позицій спрайта
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
//	//геттери для розміру текстури
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
//	//деструктор
//	~my_texture()
//	{
//		free();//очищуємо пам'ять
//	}
//};
//
////завантаження кількості кліків по екрані з файлу
//int load_counter()
//{
//	int counter = 0;
//	//відкриваємо файл
//	SDL_RWops* file = SDL_RWFromFile("counter.bin", "r+b");
//
//	//якщо файл не відкрито
//	if (file == 0)
//	{
//		//створюємо новий і записуємо туди 0
//		SDL_RWops* new_file = SDL_RWFromFile("counter.bin", "w+b");
//		SDL_RWwrite(new_file, &counter, sizeof(int), 1);
//		SDL_RWclose(new_file);
//		//та повертаємо 0
//		return counter;
//	}
//
//	//інакше зчитуємо значення та повертаємо його
//	SDL_RWread(file, &counter, sizeof(int), 1);
//	SDL_RWclose(file);
//	return counter;
//}
//
//
////збереження результатів
//void save_result(int count)
//{
//	//запис у файл кількості кліків
//	SDL_RWops* file = SDL_RWFromFile("counter.bin", "w+b");
//	SDL_RWwrite(file, &count, sizeof(int), 1);
//	SDL_RWclose(file);
//}
//
//int main(int arc,char**argv)
//{
//	srand(time(NULL));
//	SDL_Init(SDL_INIT_VIDEO|SDL_INIT_AUDIO);
//	IMG_Init(IMG_INIT_PNG);
//	TTF_Init();
//
//	//створюємо змінні для роботи програми
//	SDL_Window* main_window = SDL_CreateWindow("task", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, screen_width, screen_height, SDL_WINDOW_SHOWN| SDL_WINDOW_RESIZABLE);
//	SDL_Renderer* main_renderer = SDL_CreateRenderer(main_window, -1, SDL_RENDERER_ACCELERATED | SDL_RENDERER_PRESENTVSYNC);
//
//	my_texture texture;
//
//	int counter = load_counter();//кількість кліків з попереднього запуску програми
//
//	SDL_Color text_color = { 0,0,0 };
//	TTF_Font* font = TTF_OpenFont("lazy.ttf", 50);
//
//
//	SDL_SetRenderDrawColor(main_renderer, 0, 255, 255, 255);
//	SDL_RenderClear(main_renderer);
//	texture.load_from_text(std::to_string(counter), font, main_renderer, text_color);
//
//	//промальовуємо текстуру
//	texture.render(main_renderer, (screen_width - texture.get_width()) / 2, 0);
//
//	SDL_RenderPresent(main_renderer);
//
//	//доки не закриємо програму
//	SDL_Event events;
//	bool exit = false;
//	while (!exit)
//	{
//		while (SDL_PollEvent(&events) != 0)
//		{
//			if (events.type == SDL_QUIT)
//			{
//				exit = true;
//				//якщо закрили прошраму, зберігаємо результат
//				save_result(counter);
//				break;
//			}
//			else
//				//якщо натиснуто на кнопку миші, збільшуємо кількість кліків та промальовуємо текстуру
//				if (events.type == SDL_MOUSEBUTTONUP)
//				{
//					++counter;
//
//					SDL_SetRenderDrawColor(main_renderer, 0, 255, 255, 255);
//					SDL_RenderClear(main_renderer);
//					texture.load_from_text(std::to_string(counter), font, main_renderer, text_color);
//					texture.render(main_renderer, (screen_width- texture.get_width())/2, 0);
//					SDL_RenderPresent(main_renderer);
//				}		
//		}
//	}
//
//	//звільнення пам'яті
//	texture.free();
//	SDL_DestroyRenderer(main_renderer);
//	SDL_DestroyWindow(main_window);
//	TTF_CloseFont(font);
//	font = NULL;
//	main_renderer = NULL;
//	main_window = NULL;
//	SDL_Quit();
//	IMG_Quit();
//	return 0;
//}
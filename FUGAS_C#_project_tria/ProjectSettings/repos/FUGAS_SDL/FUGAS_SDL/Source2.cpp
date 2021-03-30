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
////генерація випадкової позиції на екрані для зображення
//SDL_Rect generate_position(int img_height,int img_width)
//{
//	int center_x = rand()% (screen_width- img_width) + img_width/2;
//	int center_y = rand() % (screen_height - img_height) + img_height / 2;
//	return { center_x- img_width / 2,center_y - img_height / 2,center_x + img_width / 2,center_y + img_height / 2 };
//}
//
////перевірка чи позиція миші на зображенні
//bool check_positions(my_texture *img)
//{
//	int x, y;
//	SDL_GetMouseState(&x,&y);
//	return img->get_position_x() <= x && img->get_position_x() + img->get_width() >= x && img->get_position_y() <= y && img->get_position_y() + img->get_height() >= y;
//}
//
////завантаження найменшого часу з файлу
//int get_best_time()
//{
//	//відкриваємо файл
//	SDL_RWops* file = NULL;
//	file = SDL_RWFromFile("time.bin", "r+b");
//	int best_result=-2;
//
//	//якщо файл не відкрито
//	if (file == NULL)
//	{
//		//створюємо його та записуємо в нього -2
//		file= SDL_RWFromFile("time.bin", "w+b");
//		SDL_RWwrite(file, &best_result, sizeof(best_result), 1);
//		SDL_RWclose(file);
//		return 0;
//	}
//	
//	//інакше зчитуємо дані і повертаємо їх
//	SDL_RWread(file, &best_result, sizeof(best_result), 1);
//	SDL_RWclose(file);
//
//	if (best_result < 0)
//		return 0;
//
//	return best_result;
//}
//
////збереження результатів
//void save_time(int time)
//{
//	//відкриваємо файл та зчитуємо з нього поточне збережене значення
//	SDL_RWops* file = SDL_RWFromFile("time.bin", "r+b");
//	int a=0;
//	SDL_RWread(file, &a, sizeof(a), 1);
//	SDL_RWclose(file);
//
//	//якщо воно від'ємне аббо поточний результат менший за збережене значення, записуємо поточний результат
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
//	//створюємо змінні для роботи програми
//	SDL_Window* main_window = SDL_CreateWindow("task", SDL_WINDOWPOS_UNDEFINED, SDL_WINDOWPOS_UNDEFINED, screen_width, screen_height, SDL_WINDOW_SHOWN);
//	SDL_Renderer* main_renderer = SDL_CreateRenderer(main_window, -1, SDL_RENDERER_ACCELERATED | SDL_RENDERER_PRESENTVSYNC);
//
//	my_texture timer;//таймер
//	my_texture texture_image;//текстура
//	my_texture best_result;//найкращий результат
//
//	//шрифт тексту
//	TTF_Font* font = TTF_OpenFont("lazy.ttf", 70);
//
//	//колір тексту
//	SDL_Color text_color = { 0,0,0 };
//
//	//завантажуємо текстури 
//	timer.load_from_text("5", font, main_renderer, text_color);
//	best_result.load_from_text(("Best: "+std::to_string(get_best_time())).c_str(), font, main_renderer, text_color);
//	texture_image.load_from_file("purebackground.png", main_renderer);
//
//	texture_image.set_color(255, 0, 0);
//
//	SDL_Rect image_pos = generate_position(texture_image.get_height(), texture_image.get_width());
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
//				break;
//			}
//			else
//			{
//				//якщо натиснуто на зображення, зберігаємо результат тп виходимо з програми
//				if (events.type == SDL_MOUSEBUTTONUP && check_positions(&texture_image))
//				{
//					save_time(SDL_GetTicks()/1000);
//					exit = true;
//					break;
//				}
//			}
//		}
//		//виводимо на екран текстури доки не вийдемо з програми
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
//	//звільнення пам'яті
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
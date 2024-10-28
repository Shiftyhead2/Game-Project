shader_type canvas_item;

uniform vec2 resolution = vec2(1280.0, 720.0);
uniform sampler2D noise_texture: filter_nearest, repeat_enable;
uniform float density: hint_range(1.0, 100.0, 0.1) = 20.0;
uniform float blink_intensity: hint_range(0.0, 1.0, 0.01) = 0.5;  // Control blinking intensity
uniform float blink_speed_range: hint_range(0.0, 10.0, 0.1) = 5.0; // Range of blinking speeds

float random(vec2 st) {
    return fract(sin(dot(st, vec2(12.9898, 78.233))) * 43758.5453);
}

vec3 get_star_color(vec2 st) {
    // Generate a unique color based on the coordinates
    float r = random(st + vec2(0.0, 0.0));
    float g = random(st + vec2(1.0, 0.0));
    float b = random(st + vec2(0.0, 1.0));
    return vec3(r, g, b);
}

void fragment() {
    // Initialize base background color to black
    vec3 background_color = vec3(0.0);

    // Called for every pixel the material is visible on.
    vec2 uv = UV;
    uv.x *= resolution.x / resolution.y;

    vec3 star_color = vec3(0.0); // Accumulate star colors

    // Unique star coordinates based on UV and layer
    vec2 unique_coords = uv * resolution.xy; // Scaling UV for better randomness

    // Generate a static color for each star based on its position
    vec3 star_color_static = get_star_color(unique_coords);

    // Calculate star presence with noise
    float star_noise = texture(noise_texture, uv).r;
    float star_brightness = pow(star_noise, density);

    // Random blinking effect
    float random_seed = random(unique_coords); // Unique random seed for each star
    float blink_speed = blink_speed_range * random_seed; // Varying blinking speed
    float blink = 0.5 + 0.5 * sin(TIME * blink_speed); // Varying frequency
    blink = mix(1.0, blink, blink_intensity); // Apply blinking intensity

    star_brightness *= blink;

    // Only add the color where stars exist
    if (step(0.2, star_brightness) > 0.0) {
        star_color += star_color_static * star_brightness;
    }

    // Output the final color, ensuring the background stays black
    COLOR = vec4(background_color + star_color, 1.0);
}

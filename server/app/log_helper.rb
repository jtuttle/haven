module LogHelper
  def log(chan, msg)
    time = Time.now.getutc.to_s.gsub(/ +UTC$/, '')
    formatted = "#{time} [#{chan}] #{msg}"
    puts formatted
    $stdout.flush
  end
end

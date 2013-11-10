class Client < EventMachine::Connection
  include LogHelper

  attr_accessor :server, :id
  attr_accessor :name, :pos

  def initialize(server)
    self.server = server
    self.name = 'noname'
    self.pos = [0,0,0]
  end

  def post_init
    server.register self
  end

  def receive_data(rawdata)
    rawdata.split("\n").each do |line|
      log :debug, "[#{id}] received: #{line}"
      data = line.split(' ')
      cmd = (data.shift || '').gsub('-', '_').downcase
      if cmd && respond_to?("on_#{cmd}")
        begin
          send("on_#{cmd}", *data)
        rescue ArgumentError => e
          puts e.message
        end
      else
        reply "error what?"
      end
    end
  end

  def unbind
    server.unregister self
  end

  def broadcast(*args)
    log "client-#{id}", "broadcast: #{args.join(' ')}"
    server.broadcast(*args)
  end

  def reply(*args)
    log "client-#{id}", "sent: #{args.join(' ')}"
    msg = args.join(' ')
    msg += "\n" unless msg.end_with?("\n")
    send_data(msg)
  end

  def on_set_position(px, py, pz)
    self.pos = [px, py, pz]
    broadcast "#{id} position #{pos.join(' ')}"
  end

  def on_get_world
    messages = server.clients.map{|c|
      "#{c.id} position #{c.pos.join(' ')}"
    }
    reply(messages.join("\n"))
  end
end
